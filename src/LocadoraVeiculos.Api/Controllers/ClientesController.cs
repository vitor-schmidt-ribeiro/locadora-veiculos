using LocadoraVeiculos.Api.DTOs;
using LocadoraVeiculos.Domain.Entities;
using LocadoraVeiculos.Infrastructure.Context;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LocadoraVeiculos.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ClientesController : ControllerBase
{
    private readonly LocadoraDbContext _context;

    public ClientesController(LocadoraDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ClienteDto>>> ObterTodos()
    {
        var clientes = await _context.Clientes
            .AsNoTracking()
            .Select(c => new ClienteDto
            {
                Id = c.Id,
                Nome = c.Nome,
                CPF = c.CPF,
                Email = c.Email,
                Telefone = c.Telefone,
                CNH = c.CNH,
                DataCadastro = c.DataCadastro
            })
            .ToListAsync();

        return Ok(clientes);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ClienteDto>> ObterPorId(int id)
    {
        var cliente = await _context.Clientes
            .AsNoTracking()
            .Where(c => c.Id == id)
            .Select(c => new ClienteDto
            {
                Id = c.Id,
                Nome = c.Nome,
                CPF = c.CPF,
                Email = c.Email,
                Telefone = c.Telefone,
                CNH = c.CNH,
                DataCadastro = c.DataCadastro
            })
            .FirstOrDefaultAsync();

        if (cliente == null)
            return NotFound(new { mensagem = $"Cliente com ID {id} não encontrado." });

        return Ok(cliente);
    }

    [HttpPost]
    public async Task<ActionResult<ClienteDto>> Criar([FromBody] CriarClienteDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var cpfExiste = await _context.Clientes.AnyAsync(c => c.CPF == dto.CPF.Trim());
        if (cpfExiste)
            return Conflict(new { mensagem = "Já existe um cliente cadastrado com este CPF." });

        var emailExiste = await _context.Clientes.AnyAsync(c => c.Email.ToLower() == dto.Email.Trim().ToLower());
        if (emailExiste)
            return Conflict(new { mensagem = "Já existe um cliente cadastrado com este e-mail." });

        var cnhExiste = await _context.Clientes.AnyAsync(c => c.CNH == dto.CNH.Trim());
        if (cnhExiste)
            return Conflict(new { mensagem = "Já existe um cliente cadastrado com esta CNH." });

        var cliente = new Cliente
        {
            Nome = dto.Nome.Trim(),
            CPF = dto.CPF.Trim(),
            Email = dto.Email.Trim().ToLower(),
            Telefone = dto.Telefone.Trim(),
            CNH = dto.CNH.Trim(),
            DataCadastro = DateTime.UtcNow
        };

        _context.Clientes.Add(cliente);
        await _context.SaveChangesAsync();

        var retorno = new ClienteDto
        {
            Id = cliente.Id,
            Nome = cliente.Nome,
            CPF = cliente.CPF,
            Email = cliente.Email,
            Telefone = cliente.Telefone,
            CNH = cliente.CNH,
            DataCadastro = cliente.DataCadastro
        };

        return CreatedAtAction(nameof(ObterPorId), new { id = cliente.Id }, retorno);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Atualizar(int id, [FromBody] AtualizarClienteDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var cliente = await _context.Clientes.FindAsync(id);
        if (cliente == null)
            return NotFound(new { mensagem = $"Cliente com ID {id} não encontrado." });

        var cpfExiste = await _context.Clientes.AnyAsync(c => c.CPF == dto.CPF.Trim() && c.Id != id);
        if (cpfExiste)
            return Conflict(new { mensagem = "Já existe outro cliente cadastrado com este CPF." });

        var emailExiste = await _context.Clientes.AnyAsync(c => c.Email.ToLower() == dto.Email.Trim().ToLower() && c.Id != id);
        if (emailExiste)
            return Conflict(new { mensagem = "Já existe outro cliente cadastrado com este e-mail." });

        var cnhExiste = await _context.Clientes.AnyAsync(c => c.CNH == dto.CNH.Trim() && c.Id != id);
        if (cnhExiste)
            return Conflict(new { mensagem = "Já existe outro cliente cadastrado com esta CNH." });

        cliente.Nome = dto.Nome.Trim();
        cliente.CPF = dto.CPF.Trim();
        cliente.Email = dto.Email.Trim().ToLower();
        cliente.Telefone = dto.Telefone.Trim();
        cliente.CNH = dto.CNH.Trim();

        await _context.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Deletar(int id)
    {
        var cliente = await _context.Clientes
            .Include(c => c.Alugueis)
            .FirstOrDefaultAsync(c => c.Id == id);

        if (cliente == null)
            return NotFound(new { mensagem = $"Cliente com ID {id} não encontrado." });

        if (cliente.Alugueis.Any())
            return BadRequest(new { mensagem = "Não é possível excluir o cliente pois existem aluguéis associados a ele." });

        _context.Clientes.Remove(cliente);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    // FILTRO 3 (LEFT JOIN entre Clientes e Alugueis)
    [HttpGet("relatorio-locacoes")]
    public async Task<ActionResult<IEnumerable<RelatorioClienteLocacoesDto>>> ObterRelatorioClientes()
    {
        var dados = await (from c in _context.Clientes
                           join a in _context.Alugueis on c.Id equals a.ClienteId into grupoAlugueis
                           from aluguel in grupoAlugueis.DefaultIfEmpty()
                           select new
                           {
                               ClienteId = c.Id,
                               Nome = c.Nome,
                               CPF = c.CPF,
                               Email = c.Email,
                               AluguelId = (int?)aluguel.Id,
                               ValorTotal = (decimal?)aluguel.ValorTotal,
                               DataInicio = (DateTime?)aluguel.DataInicio
                           })
                           .AsNoTracking()
                           .ToListAsync();

        var relatorio = dados
            .GroupBy(x => new { x.ClienteId, x.Nome, x.CPF, x.Email })
            .Select(g => new RelatorioClienteLocacoesDto
            {
                ClienteId = g.Key.ClienteId,
                Nome = g.Key.Nome,
                CPF = g.Key.CPF,
                Email = g.Key.Email,
                TotalAlugueis = g.Count(x => x.AluguelId.HasValue),
                TotalGasto = g.Where(x => x.AluguelId.HasValue).Sum(x => x.ValorTotal ?? 0),
                UltimaLocacao = g.Where(x => x.DataInicio.HasValue).Max(x => x.DataInicio)
            })
            .OrderByDescending(x => x.TotalAlugueis)
            .ToList();

        return Ok(relatorio);
    }
}
