using LocadoraVeiculos.Api.DTOs;
using LocadoraVeiculos.Domain.Entities;
using LocadoraVeiculos.Infrastructure.Context;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LocadoraVeiculos.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class FabricantesController : ControllerBase
{
    private readonly LocadoraDbContext _context;

    public FabricantesController(LocadoraDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<FabricanteDto>>> ObterTodos()
    {
        var fabricantes = await _context.Fabricantes
            .AsNoTracking()
            .Select(f => new FabricanteDto
            {
                Id = f.Id,
                Nome = f.Nome,
                PaisOrigem = f.PaisOrigem
            })
            .ToListAsync();

        return Ok(fabricantes);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<FabricanteDto>> ObterPorId(int id)
    {
        var fabricante = await _context.Fabricantes
            .AsNoTracking()
            .Where(f => f.Id == id)
            .Select(f => new FabricanteDto
            {
                Id = f.Id,
                Nome = f.Nome,
                PaisOrigem = f.PaisOrigem
            })
            .FirstOrDefaultAsync();

        if (fabricante == null)
            return NotFound(new { mensagem = $"Fabricante com ID {id} não encontrado." });

        return Ok(fabricante);
    }

    [HttpPost]
    public async Task<ActionResult<FabricanteDto>> Criar([FromBody] CriarFabricanteDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var fabricante = new Fabricante
        {
            Nome = dto.Nome.Trim(),
            PaisOrigem = dto.PaisOrigem.Trim()
        };

        _context.Fabricantes.Add(fabricante);
        await _context.SaveChangesAsync();

        var retorno = new FabricanteDto
        {
            Id = fabricante.Id,
            Nome = fabricante.Nome,
            PaisOrigem = fabricante.PaisOrigem
        };

        return CreatedAtAction(nameof(ObterPorId), new { id = fabricante.Id }, retorno);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Atualizar(int id, [FromBody] AtualizarFabricanteDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var fabricante = await _context.Fabricantes.FindAsync(id);
        if (fabricante == null)
            return NotFound(new { mensagem = $"Fabricante com ID {id} não encontrado." });

        fabricante.Nome = dto.Nome.Trim();
        fabricante.PaisOrigem = dto.PaisOrigem.Trim();

        await _context.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Deletar(int id)
    {
        var fabricante = await _context.Fabricantes
            .Include(f => f.Veiculos)
            .FirstOrDefaultAsync(f => f.Id == id);

        if (fabricante == null)
            return NotFound(new { mensagem = $"Fabricante com ID {id} não encontrado." });

        if (fabricante.Veiculos.Any())
            return BadRequest(new { mensagem = "Não é possível excluir o fabricante pois existem veículos vinculados a ele." });

        _context.Fabricantes.Remove(fabricante);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}
