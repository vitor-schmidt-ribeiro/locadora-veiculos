using LocadoraVeiculos.Api.DTOs;
using LocadoraVeiculos.Domain.Entities;
using LocadoraVeiculos.Infrastructure.Context;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LocadoraVeiculos.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CategoriasController : ControllerBase
{
    private readonly LocadoraDbContext _context;

    public CategoriasController(LocadoraDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<CategoriaDto>>> ObterTodas()
    {
        var categorias = await _context.Categorias
            .AsNoTracking()
            .Select(c => new CategoriaDto
            {
                Id = c.Id,
                Nome = c.Nome,
                Descricao = c.Descricao,
                ValorDiariaBase = c.ValorDiariaBase
            })
            .ToListAsync();

        return Ok(categorias);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<CategoriaDto>> ObterPorId(int id)
    {
        var categoria = await _context.Categorias
            .AsNoTracking()
            .Where(c => c.Id == id)
            .Select(c => new CategoriaDto
            {
                Id = c.Id,
                Nome = c.Nome,
                Descricao = c.Descricao,
                ValorDiariaBase = c.ValorDiariaBase
            })
            .FirstOrDefaultAsync();

        if (categoria == null)
            return NotFound(new { mensagem = $"Categoria com ID {id} não encontrada." });

        return Ok(categoria);
    }

    [HttpPost]
    public async Task<ActionResult<CategoriaDto>> Criar([FromBody] CriarCategoriaDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var nomeExiste = await _context.Categorias
            .AnyAsync(c => c.Nome.ToLower() == dto.Nome.Trim().ToLower());

        if (nomeExiste)
            return Conflict(new { mensagem = "Já existe uma categoria cadastrada com este nome." });

        var categoria = new Categoria
        {
            Nome = dto.Nome.Trim(),
            Descricao = dto.Descricao?.Trim(),
            ValorDiariaBase = dto.ValorDiariaBase
        };

        _context.Categorias.Add(categoria);
        await _context.SaveChangesAsync();

        var retorno = new CategoriaDto
        {
            Id = categoria.Id,
            Nome = categoria.Nome,
            Descricao = categoria.Descricao,
            ValorDiariaBase = categoria.ValorDiariaBase
        };

        return CreatedAtAction(nameof(ObterPorId), new { id = categoria.Id }, retorno);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Atualizar(int id, [FromBody] AtualizarCategoriaDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var categoria = await _context.Categorias.FindAsync(id);
        if (categoria == null)
            return NotFound(new { mensagem = $"Categoria com ID {id} não encontrada." });

        var nomeExiste = await _context.Categorias
            .AnyAsync(c => c.Nome.ToLower() == dto.Nome.Trim().ToLower() && c.Id != id);

        if (nomeExiste)
            return Conflict(new { mensagem = "Já existe outra categoria com este nome." });

        categoria.Nome = dto.Nome.Trim();
        categoria.Descricao = dto.Descricao?.Trim();
        categoria.ValorDiariaBase = dto.ValorDiariaBase;

        await _context.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Deletar(int id)
    {
        var categoria = await _context.Categorias
            .Include(c => c.Veiculos)
            .FirstOrDefaultAsync(c => c.Id == id);

        if (categoria == null)
            return NotFound(new { mensagem = $"Categoria com ID {id} não encontrada." });

        if (categoria.Veiculos.Any())
            return BadRequest(new { mensagem = "Não é possível excluir a categoria pois existem veículos vinculados a ela." });

        _context.Categorias.Remove(categoria);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}
