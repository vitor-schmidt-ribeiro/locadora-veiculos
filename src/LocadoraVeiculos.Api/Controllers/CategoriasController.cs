using LocadoraVeiculos.Api.DTOs;
using LocadoraVeiculos.Domain.Entities;
using LocadoraVeiculos.Infrastructure.Context;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LocadoraVeiculos.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class CategoriasController : ControllerBase
{
    private readonly LocadoraDbContext _context;

    public CategoriasController(LocadoraDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Lista todas as categorias de veículos cadastradas no sistema.
    /// </summary>
    /// <returns>Coleção de categorias com informações de valor base de diária.</returns>
    /// <response code="200">Lista de categorias retornada com sucesso.</response>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<CategoriaDto>), StatusCodes.Status200OK)]
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

    /// <summary>
    /// Obtém os dados de uma categoria específica pelo identificador.
    /// </summary>
    /// <param name="id">Identificador único da categoria.</param>
    /// <returns>Dados da categoria solicitada.</returns>
    /// <response code="200">Categoria encontrada com sucesso.</response>
    /// <response code="404">Categoria não encontrada.</response>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(CategoriaDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
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

    /// <summary>
    /// Cadastra uma nova categoria de veículo no sistema.
    /// </summary>
    /// <param name="dto">Dados para inclusão da categoria.</param>
    /// <returns>Dados da categoria recém-cadastrada.</returns>
    /// <response code="201">Categoria criada com sucesso.</response>
    /// <response code="400">Dados inválidos fornecidos.</response>
    /// <response code="409">Conflito por nome de categoria já existente.</response>
    [HttpPost]
    [ProducesResponseType(typeof(CategoriaDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
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

    /// <summary>
    /// Atualiza os dados de uma categoria de veículo existente.
    /// </summary>
    /// <param name="id">Identificador único da categoria.</param>
    /// <param name="dto">Dados atualizados da categoria.</param>
    /// <response code="204">Categoria atualizada com sucesso.</response>
    /// <response code="400">Dados inválidos fornecidos.</response>
    /// <response code="404">Categoria não encontrada.</response>
    /// <response code="409">Conflito por nome já utilizado em outra categoria.</response>
    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
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

    /// <summary>
    /// Remove uma categoria do banco de dados.
    /// </summary>
    /// <param name="id">Identificador da categoria a ser excluída.</param>
    /// <response code="204">Categoria excluída com sucesso.</response>
    /// <response code="400">Não é possível excluir a categoria pois existem veículos associados a ela.</response>
    /// <response code="404">Categoria não encontrada.</response>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
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
