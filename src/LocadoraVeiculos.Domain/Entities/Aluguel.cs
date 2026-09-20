using LocadoraVeiculos.Domain.Enums;

namespace LocadoraVeiculos.Domain.Entities;

public class Aluguel
{
    public int Id { get; set; }

    public int ClienteId { get; set; }
    public Cliente Cliente { get; set; } = null!;

    public int VeiculoId { get; set; }
    public Veiculo Veiculo { get; set; } = null!;

    public DateTime DataInicio { get; set; }
    public DateTime DataPrevistaDevolucao { get; set; }
    public DateTime? DataDevolucaoEfetiva { get; set; }

    public int KmInicial { get; set; }
    public int? KmFinal { get; set; }

    public decimal ValorDiaria { get; set; }
    public decimal ValorTotal { get; set; }

    public StatusAluguel Status { get; set; } = StatusAluguel.Ativo;

    public Pagamento? Pagamento { get; set; }
}
