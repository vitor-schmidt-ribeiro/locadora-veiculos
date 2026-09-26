using LocadoraVeiculos.Api.DTOs;
using LocadoraVeiculos.Domain.Entities;
using LocadoraVeiculos.Domain.Enums;
using LocadoraVeiculos.Infrastructure.Context;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LocadoraVeiculos.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class VeiculosController : ControllerBase
{
    private readonly LocadoraDbContext _context;

    public VeiculosController(LocadoraDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<VeiculoDto>>> ObterTodos()
    {
        var veiculos = await _context.Veiculos
            .AsNoTracking()
            .Include(v => v.Fabricante)
            .Include(v => v.Categoria)
            .Select(v => new VeiculoDto
            {
                Id = v.Id,
                Modelo = v.Modelo,
                AnoFabricacao = v.AnoFabricacao,
                Quilometragem = v.Quilometragem,
                Placa = v.Placa,
                Cor = v.Cor,
                Status = v.Status,
                FabricanteId = v.FabricanteId,
                FabricanteNome = v.Fabricante.Nome,
                CategoriaId = v.CategoriaId,
                CategoriaNome = v.Categoria.Nome
            })
            .ToListAsync();

        return Ok(veiculos);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<VeiculoDto>> ObterPorId(int id)
    {
        var veiculo = await _context.Veiculos
            .AsNoTracking()
            .Include(v => v.Fabricante)
            .Include(v => v.Categoria)
            .Where(v => v.Id == id)
            .Select(v => new VeiculoDto
            {
                Id = v.Id,
                Modelo = v.Modelo,
                AnoFabricacao = v.AnoFabricacao,
                Quilometragem = v.Quilometragem,
                Placa = v.Placa,
                Cor = v.Cor,
                Status = v.Status,
                FabricanteId = v.FabricanteId,
                FabricanteNome = v.Fabricante.Nome,
                CategoriaId = v.CategoriaId,
                CategoriaNome = v.Categoria.Nome
            })
            .FirstOrDefaultAsync();

        if (veiculo == null)
            return NotFound(new { mensagem = $"Veículo com ID {id} não encontrado." });

        return Ok(veiculo);
    }

    [HttpPost]
    public async Task<ActionResult<VeiculoDto>> Criar([FromBody] CriarVeiculoDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var placaExiste = await _context.Veiculos
            .AnyAsync(v => v.Placa.ToUpper() == dto.Placa.Trim().ToUpper());

        if (placaExiste)
            return Conflict(new { mensagem = "Já existe um veículo cadastrado com esta placa." });

        var fabricanteExiste = await _context.Fabricantes.AnyAsync(f => f.Id == dto.FabricanteId);
        if (!fabricanteExiste)
            return BadRequest(new { mensagem = $"Fabricante com ID {dto.FabricanteId} não existe." });

        var categoriaExiste = await _context.Categorias.AnyAsync(c => c.Id == dto.CategoriaId);
        if (!categoriaExiste)
            return BadRequest(new { mensagem = $"Categoria com ID {dto.CategoriaId} não existe." });

        var veiculo = new Veiculo
        {
            Modelo = dto.Modelo.Trim(),
            AnoFabricacao = dto.AnoFabricacao,
            Quilometragem = dto.Quilometragem,
            Placa = dto.Placa.Trim().ToUpper(),
            Cor = dto.Cor.Trim(),
            Status = StatusVeiculo.Disponivel,
            FabricanteId = dto.FabricanteId,
            CategoriaId = dto.CategoriaId
        };

        _context.Veiculos.Add(veiculo);
        await _context.SaveChangesAsync();

        var fabricante = await _context.Fabricantes.FindAsync(veiculo.FabricanteId);
        var categoria = await _context.Categorias.FindAsync(veiculo.CategoriaId);

        var retorno = new VeiculoDto
        {
            Id = veiculo.Id,
            Modelo = veiculo.Modelo,
            AnoFabricacao = veiculo.AnoFabricacao,
            Quilometragem = veiculo.Quilometragem,
            Placa = veiculo.Placa,
            Cor = veiculo.Cor,
            Status = veiculo.Status,
            FabricanteId = veiculo.FabricanteId,
            FabricanteNome = fabricante?.Nome ?? string.Empty,
            CategoriaId = veiculo.CategoriaId,
            CategoriaNome = categoria?.Nome ?? string.Empty
        };

        return CreatedAtAction(nameof(ObterPorId), new { id = veiculo.Id }, retorno);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Atualizar(int id, [FromBody] AtualizarVeiculoDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var veiculo = await _context.Veiculos.FindAsync(id);
        if (veiculo == null)
            return NotFound(new { mensagem = $"Veículo com ID {id} não encontrado." });

        var placaExiste = await _context.Veiculos
            .AnyAsync(v => v.Placa.ToUpper() == dto.Placa.Trim().ToUpper() && v.Id != id);

        if (placaExiste)
            return Conflict(new { mensagem = "Já existe outro veículo cadastrado com esta placa." });

        var fabricanteExiste = await _context.Fabricantes.AnyAsync(f => f.Id == dto.FabricanteId);
        if (!fabricanteExiste)
            return BadRequest(new { mensagem = $"Fabricante com ID {dto.FabricanteId} não existe." });

        var categoriaExiste = await _context.Categorias.AnyAsync(c => c.Id == dto.CategoriaId);
        if (!categoriaExiste)
            return BadRequest(new { mensagem = $"Categoria com ID {dto.CategoriaId} não existe." });

        veiculo.Modelo = dto.Modelo.Trim();
        veiculo.AnoFabricacao = dto.AnoFabricacao;
        veiculo.Quilometragem = dto.Quilometragem;
        veiculo.Placa = dto.Placa.Trim().ToUpper();
        veiculo.Cor = dto.Cor.Trim();
        veiculo.Status = dto.Status;
        veiculo.FabricanteId = dto.FabricanteId;
        veiculo.CategoriaId = dto.CategoriaId;

        await _context.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Deletar(int id)
    {
        var veiculo = await _context.Veiculos
            .Include(v => v.Alugueis)
            .FirstOrDefaultAsync(v => v.Id == id);

        if (veiculo == null)
            return NotFound(new { mensagem = $"Veículo com ID {id} não encontrado." });

        if (veiculo.Alugueis.Any())
            return BadRequest(new { mensagem = "Não é possível excluir o veículo pois existem aluguéis associados a ele." });

        _context.Veiculos.Remove(veiculo);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    // FILTRO 1 (INNER JOIN entre Veiculos, Categorias e Fabricantes)
    [HttpGet("disponiveis")]
    public async Task<ActionResult<IEnumerable<VeiculoDisponivelDto>>> FiltrarDisponiveis(
        [FromQuery] int? categoriaId,
        [FromQuery] int? fabricanteId)
    {
        var consulta = from v in _context.Veiculos
                       join c in _context.Categorias on v.CategoriaId equals c.Id
                       join f in _context.Fabricantes on v.FabricanteId equals f.Id
                       where v.Status == StatusVeiculo.Disponivel
                       select new { v, c, f };

        if (categoriaId.HasValue)
            consulta = consulta.Where(x => x.c.Id == categoriaId.Value);

        if (fabricanteId.HasValue)
            consulta = consulta.Where(x => x.f.Id == fabricanteId.Value);

        var resultado = await consulta
            .AsNoTracking()
            .Select(x => new VeiculoDisponivelDto
            {
                VeiculoId = x.v.Id,
                Modelo = x.v.Modelo,
                AnoFabricacao = x.v.AnoFabricacao,
                Placa = x.v.Placa,
                Cor = x.v.Cor,
                Quilometragem = x.v.Quilometragem,
                Fabricante = x.f.Nome,
                PaisFabricante = x.f.PaisOrigem,
                Categoria = x.c.Nome,
                ValorDiariaBase = x.c.ValorDiariaBase
            })
            .ToListAsync();

        return Ok(resultado);
    }

    // FILTRO 4 (LEFT JOIN entre Veiculos e Alugueis, com INNER JOIN em Fabricante e Categoria)
    [HttpGet("relatorio-frota")]
    public async Task<ActionResult<IEnumerable<RelatorioFrotaDto>>> ObterRelatorioFrota()
    {
        var relatorio = await (from v in _context.Veiculos
                               join f in _context.Fabricantes on v.FabricanteId equals f.Id
                               join c in _context.Categorias on v.CategoriaId equals c.Id
                               join a in _context.Alugueis on v.Id equals a.VeiculoId into grupoAlugueis
                               from aluguel in grupoAlugueis.DefaultIfEmpty()
                               select new
                               {
                                   VeiculoId = v.Id,
                                   Modelo = v.Modelo,
                                   Placa = v.Placa,
                                   Fabricante = f.Nome,
                                   Categoria = c.Nome,
                                   Status = v.Status,
                                   Quilometragem = v.Quilometragem,
                                   AluguelId = (int?)aluguel.Id,
                                   ValorTotal = (decimal?)aluguel.ValorTotal,
                                   KmFinal = (int?)aluguel.KmFinal
                               })
                               .AsNoTracking()
                               .ToListAsync();

        var agrupado = relatorio
            .GroupBy(x => new { x.VeiculoId, x.Modelo, x.Placa, x.Fabricante, x.Categoria, x.Status, x.Quilometragem })
            .Select(g => new RelatorioFrotaDto
            {
                VeiculoId = g.Key.VeiculoId,
                Modelo = g.Key.Modelo,
                Placa = g.Key.Placa,
                Fabricante = g.Key.Fabricante,
                Categoria = g.Key.Categoria,
                StatusAtual = g.Key.Status,
                TotalLocacoes = g.Count(x => x.AluguelId.HasValue),
                TotalFaturado = g.Where(x => x.AluguelId.HasValue).Sum(x => x.ValorTotal ?? 0),
                UltimoKmRegistrado = g.Where(x => x.KmFinal.HasValue).Max(x => x.KmFinal) ?? g.Key.Quilometragem
            })
            .OrderByDescending(x => x.TotalLocacoes)
            .ToList();

        return Ok(agrupado);
    }
}
