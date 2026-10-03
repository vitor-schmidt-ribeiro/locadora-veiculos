using LocadoraVeiculos.Api.DTOs;
using LocadoraVeiculos.Domain.Entities;
using LocadoraVeiculos.Infrastructure.Context;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LocadoraVeiculos.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class ClientesController : ControllerBase
{
    private readonly LocadoraDbContext _context;

    public ClientesController(LocadoraDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Lista todos os clientes cadastrados.
    /// </summary>
    /// <returns>Coleção de clientes cadastrados.</returns>
    /// <response code="200">Lista de clientes retornada com sucesso.</response>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<ClienteDto>), StatusCodes.Status200OK)]
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

    /// <summary>
    /// Obtém os dados cadastrais de um cliente pelo ID.
    /// </summary>
    /// <param name="id">Identificador único do cliente.</param>
    /// <returns>Dados do cliente encontrado.</returns>
    /// <response code="200">Cliente localizado com sucesso.</response>
    /// <response code="404">Cliente não encontrado.</response>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ClienteDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
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

    /// <summary>
    /// Cadastra um novo cliente com validação de unicidade de documentos.
    /// </summary>
    /// <param name="dto">Dados para cadastro do cliente.</param>
    /// <returns>Dados do cliente cadastrado.</returns>
    /// <response code="201">Cliente cadastrado com sucesso.</response>
    /// <response code="400">Dados inválidos fornecidos.</response>
    /// <response code="409">Conflito por duplicidade de CPF, E-mail ou CNH.</response>
    [HttpPost]
    [ProducesResponseType(typeof(ClienteDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
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

    /// <summary>
    /// Atualiza os dados cadastrais de um cliente existente.
    /// </summary>
    /// <param name="id">Identificador único do cliente.</param>
    /// <param name="dto">Dados atualizados do cliente.</param>
    /// <response code="204">Cliente atualizado com sucesso.</response>
    /// <response code="400">Dados inválidos fornecidos.</response>
    /// <response code="404">Cliente não encontrado.</response>
    /// <response code="409">Conflito por documento já utilizado por outro cliente.</response>
    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
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

    /// <summary>
    /// Remove um cliente do sistema.
    /// </summary>
    /// <param name="id">Identificador do cliente.</param>
    /// <response code="204">Cliente removido com sucesso.</response>
    /// <response code="400">Não é possível excluir o cliente pois existem contratos associados.</response>
    /// <response code="404">Cliente não encontrado.</response>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
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

    /// <summary>
    /// Filtro 3 (LEFT JOIN): Retorna relatório de clientes e suas locações, abrangendo clientes sem histórico.
    /// </summary>
    /// <returns>Relatório agrupado por cliente com quantidade de aluguéis e montante financeiro gasto.</returns>
    /// <response code="200">Relatório de clientes gerado com sucesso.</response>
    [HttpGet("relatorio-locacoes")]
    [ProducesResponseType(typeof(IEnumerable<RelatorioClienteLocacoesDto>), StatusCodes.Status200OK)]
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
