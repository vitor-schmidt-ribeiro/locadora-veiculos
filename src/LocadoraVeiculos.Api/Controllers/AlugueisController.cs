using LocadoraVeiculos.Api.DTOs;
using LocadoraVeiculos.Domain.Entities;
using LocadoraVeiculos.Domain.Enums;
using LocadoraVeiculos.Infrastructure.Context;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LocadoraVeiculos.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class AlugueisController : ControllerBase
{
    private readonly LocadoraDbContext _context;

    public AlugueisController(LocadoraDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Lista todos os contratos de locação registrados.
    /// </summary>
    /// <returns>Coleção de contratos de aluguel com dados de cliente e veículo.</returns>
    /// <response code="200">Lista de aluguéis retornada com sucesso.</response>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<AluguelDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<AluguelDto>>> ObterTodos()
    {
        var alugueis = await _context.Alugueis
            .AsNoTracking()
            .Include(a => a.Cliente)
            .Include(a => a.Veiculo)
            .Select(a => new AluguelDto
            {
                Id = a.Id,
                ClienteId = a.ClienteId,
                ClienteNome = a.Cliente.Nome,
                ClienteCpf = a.Cliente.CPF,
                VeiculoId = a.VeiculoId,
                VeiculoModelo = a.Veiculo.Modelo,
                VeiculoPlaca = a.Veiculo.Placa,
                DataInicio = a.DataInicio,
                DataPrevistaDevolucao = a.DataPrevistaDevolucao,
                DataDevolucaoEfetiva = a.DataDevolucaoEfetiva,
                KmInicial = a.KmInicial,
                KmFinal = a.KmFinal,
                ValorDiaria = a.ValorDiaria,
                ValorTotal = a.ValorTotal,
                Status = a.Status
            })
            .ToListAsync();

        return Ok(alugueis);
    }

    /// <summary>
    /// Obtém os dados detalhados de um aluguel pelo identificador.
    /// </summary>
    /// <param name="id">Identificador único do contrato de aluguel.</param>
    /// <returns>Dados do contrato de aluguel.</returns>
    /// <response code="200">Contrato localizado com sucesso.</response>
    /// <response code="404">Contrato de aluguel não encontrado.</response>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(AluguelDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<AluguelDto>> ObterPorId(int id)
    {
        var aluguel = await _context.Alugueis
            .AsNoTracking()
            .Include(a => a.Cliente)
            .Include(a => a.Veiculo)
            .Where(a => a.Id == id)
            .Select(a => new AluguelDto
            {
                Id = a.Id,
                ClienteId = a.ClienteId,
                ClienteNome = a.Cliente.Nome,
                ClienteCpf = a.Cliente.CPF,
                VeiculoId = a.VeiculoId,
                VeiculoModelo = a.Veiculo.Modelo,
                VeiculoPlaca = a.Veiculo.Placa,
                DataInicio = a.DataInicio,
                DataPrevistaDevolucao = a.DataPrevistaDevolucao,
                DataDevolucaoEfetiva = a.DataDevolucaoEfetiva,
                KmInicial = a.KmInicial,
                KmFinal = a.KmFinal,
                ValorDiaria = a.ValorDiaria,
                ValorTotal = a.ValorTotal,
                Status = a.Status
            })
            .FirstOrDefaultAsync();

        if (aluguel == null)
            return NotFound(new { mensagem = $"Aluguel com ID {id} não encontrado." });

        return Ok(aluguel);
    }

    /// <summary>
    /// Cria uma nova locação de veículo para um cliente.
    /// </summary>
    /// <param name="dto">Dados para abertura da locação.</param>
    /// <returns>Dados do contrato de locação gerado.</returns>
    /// <response code="201">Locação criada com sucesso e status do veículo alterado para Alugado.</response>
    /// <response code="400">Dados inválidos fornecidos ou inconsistência de datas.</response>
    /// <response code="404">Cliente ou veículo não localizado.</response>
    /// <response code="409">Veículo indisponível para locação.</response>
    [HttpPost]
    [ProducesResponseType(typeof(AluguelDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<ActionResult<AluguelDto>> Criar([FromBody] CriarAluguelDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        if (dto.DataPrevistaDevolucao <= dto.DataInicio)
            return BadRequest(new { mensagem = "A data prevista de devolução deve ser posterior à data de início." });

        var cliente = await _context.Clientes.FindAsync(dto.ClienteId);
        if (cliente == null)
            return NotFound(new { mensagem = $"Cliente com ID {dto.ClienteId} não encontrado." });

        var veiculo = await _context.Veiculos
            .Include(v => v.Categoria)
            .FirstOrDefaultAsync(v => v.Id == dto.VeiculoId);

        if (veiculo == null)
            return NotFound(new { mensagem = $"Veículo com ID {dto.VeiculoId} não encontrado." });

        if (veiculo.Status != StatusVeiculo.Disponivel)
            return Conflict(new { mensagem = "O veículo selecionado não está disponível para locação." });

        var valorDiaria = dto.ValorDiaria.HasValue && dto.ValorDiaria > 0
            ? dto.ValorDiaria.Value
            : veiculo.Categoria.ValorDiariaBase;

        var totalDias = (int)Math.Ceiling((dto.DataPrevistaDevolucao - dto.DataInicio).TotalDays);
        if (totalDias <= 0) totalDias = 1;

        var valorTotal = totalDias * valorDiaria;

        var aluguel = new Aluguel
        {
            ClienteId = dto.ClienteId,
            VeiculoId = dto.VeiculoId,
            DataInicio = dto.DataInicio,
            DataPrevistaDevolucao = dto.DataPrevistaDevolucao,
            KmInicial = veiculo.Quilometragem,
            ValorDiaria = valorDiaria,
            ValorTotal = valorTotal,
            Status = StatusAluguel.Ativo
        };

        veiculo.Status = StatusVeiculo.Alugado;

        _context.Alugueis.Add(aluguel);
        await _context.SaveChangesAsync();

        var retorno = new AluguelDto
        {
            Id = aluguel.Id,
            ClienteId = cliente.Id,
            ClienteNome = cliente.Nome,
            ClienteCpf = cliente.CPF,
            VeiculoId = veiculo.Id,
            VeiculoModelo = veiculo.Modelo,
            VeiculoPlaca = veiculo.Placa,
            DataInicio = aluguel.DataInicio,
            DataPrevistaDevolucao = aluguel.DataPrevistaDevolucao,
            KmInicial = aluguel.KmInicial,
            ValorDiaria = aluguel.ValorDiaria,
            ValorTotal = aluguel.ValorTotal,
            Status = aluguel.Status
        };

        return CreatedAtAction(nameof(ObterPorId), new { id = aluguel.Id }, retorno);
    }

    /// <summary>
    /// Registra a devolução do veículo, calcula o valor final da locação e atualiza o odômetro da frota.
    /// </summary>
    /// <param name="id">Identificador do aluguel a ser finalizado.</param>
    /// <param name="dto">Dados de devolução (data efetiva e odômetro final).</param>
    /// <response code="200">Devolução registrada com sucesso e veículo liberado.</response>
    /// <response code="400">Dados inválidos ou inconsistência na quilometragem/data.</response>
    /// <response code="404">Aluguel não encontrado.</response>
    [HttpPut("{id}/devolucao")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> RegistrarDevolucao(int id, [FromBody] DevolucaoAluguelDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var aluguel = await _context.Alugueis
            .Include(a => a.Veiculo)
            .FirstOrDefaultAsync(a => a.Id == id);

        if (aluguel == null)
            return NotFound(new { mensagem = $"Aluguel com ID {id} não encontrado." });

        if (aluguel.Status != StatusAluguel.Ativo)
            return BadRequest(new { mensagem = "Apenas aluguéis em status ativo podem ser devolvidos." });

        if (dto.DataDevolucao < aluguel.DataInicio)
            return BadRequest(new { mensagem = "A data de devolução não pode ser anterior à data de início da locação." });

        if (dto.KmFinal < aluguel.KmInicial)
            return BadRequest(new { mensagem = "A quilometragem final não pode ser menor do que a quilometragem inicial da retirada." });

        aluguel.DataDevolucaoEfetiva = dto.DataDevolucao;
        aluguel.KmFinal = dto.KmFinal;
        aluguel.Status = StatusAluguel.Concluido;

        var diasEfetivos = (int)Math.Ceiling((dto.DataDevolucao - aluguel.DataInicio).TotalDays);
        if (diasEfetivos <= 0) diasEfetivos = 1;
        aluguel.ValorTotal = diasEfetivos * aluguel.ValorDiaria;

        aluguel.Veiculo.Quilometragem = dto.KmFinal;
        aluguel.Veiculo.Status = StatusVeiculo.Disponivel;

        await _context.SaveChangesAsync();
        return Ok(new
        {
            mensagem = "Devolução registrada com sucesso.",
            aluguelId = aluguel.Id,
            diasLocados = diasEfetivos,
            quilometragemPercorrida = dto.KmFinal - aluguel.KmInicial,
            valorFinal = aluguel.ValorTotal
        });
    }

    /// <summary>
    /// Cancela um aluguel ativo (liberando o veículo) ou exclui o registro histórico.
    /// </summary>
    /// <param name="id">Identificador do aluguel.</param>
    /// <response code="200">Aluguel ativo cancelado com sucesso.</response>
    /// <response code="204">Registro de aluguel excluído com sucesso.</response>
    /// <response code="404">Aluguel não localizado.</response>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> CancelarOuExcluir(int id)
    {
        var aluguel = await _context.Alugueis
            .Include(a => a.Veiculo)
            .Include(a => a.Pagamento)
            .FirstOrDefaultAsync(a => a.Id == id);

        if (aluguel == null)
            return NotFound(new { mensagem = $"Aluguel com ID {id} não encontrado." });

        if (aluguel.Status == StatusAluguel.Ativo)
        {
            aluguel.Status = StatusAluguel.Cancelado;
            aluguel.Veiculo.Status = StatusVeiculo.Disponivel;
            await _context.SaveChangesAsync();
            return Ok(new { mensagem = "Aluguel ativo cancelado com sucesso e veículo liberado." });
        }

        if (aluguel.Pagamento != null)
            _context.Pagamentos.Remove(aluguel.Pagamento);

        _context.Alugueis.Remove(aluguel);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    /// <summary>
    /// Filtro 2 (INNER JOIN): Retorna o histórico de locações de um cliente com dados completos de veículo e fabricante.
    /// </summary>
    /// <param name="clienteId">Identificador do cliente.</param>
    /// <param name="status">Filtro opcional pelo status da locação.</param>
    /// <returns>Histórico de locações do cliente.</returns>
    /// <response code="200">Histórico de locações retornado com sucesso.</response>
    /// <response code="404">Cliente não encontrado.</response>
    [HttpGet("cliente/{clienteId}")]
    [ProducesResponseType(typeof(IEnumerable<HistoricoAluguelClienteDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<IEnumerable<HistoricoAluguelClienteDto>>> ObterHistoricoPorCliente(
        int clienteId,
        [FromQuery] StatusAluguel? status)
    {
        var clienteExiste = await _context.Clientes.AnyAsync(c => c.Id == clienteId);
        if (!clienteExiste)
            return NotFound(new { mensagem = $"Cliente com ID {clienteId} não encontrado." });

        var consulta = from a in _context.Alugueis
                       join c in _context.Clientes on a.ClienteId equals c.Id
                       join v in _context.Veiculos on a.VeiculoId equals v.Id
                       join f in _context.Fabricantes on v.FabricanteId equals f.Id
                       where a.ClienteId == clienteId
                       select new { a, c, v, f };

        if (status.HasValue)
            consulta = consulta.Where(x => x.a.Status == status.Value);

        var resultado = await consulta
            .AsNoTracking()
            .OrderByDescending(x => x.a.DataInicio)
            .Select(x => new HistoricoAluguelClienteDto
            {
                AluguelId = x.a.Id,
                ClienteId = x.c.Id,
                ClienteNome = x.c.Nome,
                CPF = x.c.CPF,
                ModeloVeiculo = x.v.Modelo,
                PlacaVeiculo = x.v.Placa,
                Fabricante = x.f.Nome,
                DataInicio = x.a.DataInicio,
                DataPrevistaDevolucao = x.a.DataPrevistaDevolucao,
                DataDevolucaoEfetiva = x.a.DataDevolucaoEfetiva,
                KmInicial = x.a.KmInicial,
                KmFinal = x.a.KmFinal,
                ValorDiaria = x.a.ValorDiaria,
                ValorTotal = x.a.ValorTotal,
                Status = x.a.Status
            })
            .ToListAsync();

        return Ok(resultado);
    }

    /// <summary>
    /// Filtro 5 (LEFT JOIN e INNER JOIN): Retorna contratos de aluguel associados à sua situação de pagamento.
    /// </summary>
    /// <param name="statusPagamento">Filtro opcional pelo status financeiro (Pendente, Pago, Cancelado).</param>
    /// <returns>Lista de aluguéis com status de pagamento.</returns>
    /// <response code="200">Lista com status de pagamento retornada com sucesso.</response>
    [HttpGet("status-pagamento")]
    [ProducesResponseType(typeof(IEnumerable<AluguelStatusPagamentoDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<AluguelStatusPagamentoDto>>> ObterPorStatusPagamento(
        [FromQuery] StatusPagamento? statusPagamento)
    {
        var consulta = from a in _context.Alugueis
                       join c in _context.Clientes on a.ClienteId equals c.Id
                       join v in _context.Veiculos on a.VeiculoId equals v.Id
                       join p in _context.Pagamentos on a.Id equals p.AluguelId into grupoPagamentos
                       from pagamento in grupoPagamentos.DefaultIfEmpty()
                       select new
                       {
                           AluguelId = a.Id,
                           ClienteNome = c.Nome,
                           VeiculoModelo = v.Modelo,
                           Placa = v.Placa,
                           DataInicio = a.DataInicio,
                           ValorTotal = a.ValorTotal,
                           StatusAluguel = a.Status,
                           PagamentoId = (int?)pagamento.Id,
                           ValorPago = (decimal?)pagamento.ValorPago,
                           DataPagamento = (DateTime?)pagamento.DataPagamento,
                           MetodoPagamento = (MetodoPagamento?)pagamento.MetodoPagamento,
                           StatusPagamento = (StatusPagamento?)pagamento.Status
                       };

        if (statusPagamento.HasValue)
        {
            consulta = consulta.Where(x => x.StatusPagamento == statusPagamento.Value);
        }

        var dados = await consulta
            .AsNoTracking()
            .OrderByDescending(x => x.DataInicio)
            .ToListAsync();

        var resultado = dados.Select(x => new AluguelStatusPagamentoDto
        {
            AluguelId = x.AluguelId,
            ClienteNome = x.ClienteNome,
            VeiculoModelo = x.VeiculoModelo,
            Placa = x.Placa,
            DataInicio = x.DataInicio,
            ValorTotalAluguel = x.ValorTotal,
            StatusAluguel = x.StatusAluguel,
            PagamentoId = x.PagamentoId,
            ValorPago = x.ValorPago,
            DataPagamento = x.DataPagamento,
            MetodoPagamento = x.MetodoPagamento,
            StatusPagamento = x.StatusPagamento,
            SituacaoFinanceira = x.StatusPagamento == StatusPagamento.Pago
                ? "Quitado"
                : (x.PagamentoId.HasValue ? "Pendente" : "Sem Registro de Pagamento")
        }).ToList();

        return Ok(resultado);
    }
}
