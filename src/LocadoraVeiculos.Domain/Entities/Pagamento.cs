using LocadoraVeiculos.Domain.Enums;

namespace LocadoraVeiculos.Domain.Entities;

public class Pagamento
{
    public int Id { get; set; }

    public int AluguelId { get; set; }
    public Aluguel Aluguel { get; set; } = null!;

    public DateTime DataPagamento { get; set; } = DateTime.UtcNow;
    public decimal ValorPago { get; set; }
    public MetodoPagamento MetodoPagamento { get; set; } = MetodoPagamento.Pix;
    public StatusPagamento Status { get; set; } = StatusPagamento.Pago;
}
