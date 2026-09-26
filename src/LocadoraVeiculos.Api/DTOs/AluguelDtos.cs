using System.ComponentModel.DataAnnotations;
using LocadoraVeiculos.Domain.Enums;

namespace LocadoraVeiculos.Api.DTOs;

public class AluguelDto
{
    public int Id { get; set; }
    public int ClienteId { get; set; }
    public string ClienteNome { get; set; } = string.Empty;
    public string ClienteCpf { get; set; } = string.Empty;
    public int VeiculoId { get; set; }
    public string VeiculoModelo { get; set; } = string.Empty;
    public string VeiculoPlaca { get; set; } = string.Empty;
    public DateTime DataInicio { get; set; }
    public DateTime DataPrevistaDevolucao { get; set; }
    public DateTime? DataDevolucaoEfetiva { get; set; }
    public int KmInicial { get; set; }
    public int? KmFinal { get; set; }
    public decimal ValorDiaria { get; set; }
    public decimal ValorTotal { get; set; }
    public StatusAluguel Status { get; set; }
}

public class CriarAluguelDto
{
    [Required(ErrorMessage = "O cliente é obrigatório.")]
    public int ClienteId { get; set; }

    [Required(ErrorMessage = "O veículo é obrigatório.")]
    public int VeiculoId { get; set; }

    [Required(ErrorMessage = "A data de início é obrigatória.")]
    public DateTime DataInicio { get; set; }

    [Required(ErrorMessage = "A data prevista de devolução é obrigatória.")]
    public DateTime DataPrevistaDevolucao { get; set; }

    [Range(0, 100000.00, ErrorMessage = "Valor da diária inválido.")]
    public decimal? ValorDiaria { get; set; }
}

public class DevolucaoAluguelDto
{
    [Required(ErrorMessage = "A data de devolução é obrigatória.")]
    public DateTime DataDevolucao { get; set; }

    [Required(ErrorMessage = "O odômetro final é obrigatório.")]
    [Range(0, 2000000, ErrorMessage = "Quilometragem final inválida.")]
    public int KmFinal { get; set; }
}

public class HistoricoAluguelClienteDto
{
    public int AluguelId { get; set; }
    public int ClienteId { get; set; }
    public string ClienteNome { get; set; } = string.Empty;
    public string CPF { get; set; } = string.Empty;
    public string ModeloVeiculo { get; set; } = string.Empty;
    public string PlacaVeiculo { get; set; } = string.Empty;
    public string Fabricante { get; set; } = string.Empty;
    public DateTime DataInicio { get; set; }
    public DateTime DataPrevistaDevolucao { get; set; }
    public DateTime? DataDevolucaoEfetiva { get; set; }
    public int KmInicial { get; set; }
    public int? KmFinal { get; set; }
    public decimal ValorDiaria { get; set; }
    public decimal ValorTotal { get; set; }
    public StatusAluguel Status { get; set; }
}

public class AluguelStatusPagamentoDto
{
    public int AluguelId { get; set; }
    public string ClienteNome { get; set; } = string.Empty;
    public string VeiculoModelo { get; set; } = string.Empty;
    public string Placa { get; set; } = string.Empty;
    public DateTime DataInicio { get; set; }
    public decimal ValorTotalAluguel { get; set; }
    public StatusAluguel StatusAluguel { get; set; }
    public int? PagamentoId { get; set; }
    public decimal? ValorPago { get; set; }
    public DateTime? DataPagamento { get; set; }
    public MetodoPagamento? MetodoPagamento { get; set; }
    public StatusPagamento? StatusPagamento { get; set; }
    public string SituacaoFinanceira { get; set; } = string.Empty;
}
