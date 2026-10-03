using LocadoraVeiculos.Api.DTOs;
using LocadoraVeiculos.Domain.Entities;
using LocadoraVeiculos.Infrastructure.Context;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LocadoraVeiculos.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class FabricantesController : ControllerBase
{
    private readonly LocadoraDbContext _context;

    public FabricantesController(LocadoraDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Lista todos os fabricantes cadastrados no sistema.
    /// </summary>
    /// <returns>Coleção de fabricantes.</returns>
    /// <response code="200">Retorna a lista de fabricantes cadastrados.</response>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<FabricanteDto>), StatusCodes.Status200OK)]
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

    /// <summary>
    /// Obtém os dados de um fabricante específico pelo seu identificador.
    /// </summary>
    /// <param name="id">Identificador único do fabricante.</param>
    /// <returns>Dados do fabricante localizado.</returns>
    /// <response code="200">Fabricante encontrado com sucesso.</response>
    /// <response code="404">Fabricante não localizado com o ID informado.</response>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(FabricanteDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
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

    /// <summary>
    /// Cadastra um novo fabricante no banco de dados.
    /// </summary>
    /// <param name="dto">Dados para criação do fabricante.</param>
    /// <returns>Dados do fabricante criado.</returns>
    /// <response code="201">Fabricante cadastrado com sucesso.</response>
    /// <response code="400">Dados inválidos fornecidos na requisição.</response>
    [HttpPost]
    [ProducesResponseType(typeof(FabricanteDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
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

    /// <summary>
    /// Atualiza os dados de um fabricante existente.
    /// </summary>
    /// <param name="id">Identificador único do fabricante a ser atualizado.</param>
    /// <param name="dto">Novos dados cadastrais do fabricante.</param>
    /// <response code="204">Fabricante atualizado com sucesso.</response>
    /// <response code="400">Dados inválidos fornecidos na requisição.</response>
    /// <response code="404">Fabricante não localizado para o ID informado.</response>
    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
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

    /// <summary>
    /// Remove um fabricante do banco de dados.
    /// </summary>
    /// <param name="id">Identificador do fabricante.</param>
    /// <response code="204">Fabricante excluído com sucesso.</response>
    /// <response code="400">Não é possível excluir o fabricante pois existem veículos vinculados.</response>
    /// <response code="404">Fabricante não encontrado.</response>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
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
