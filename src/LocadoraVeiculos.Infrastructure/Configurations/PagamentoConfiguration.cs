using LocadoraVeiculos.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LocadoraVeiculos.Infrastructure.Configurations;

public class PagamentoConfiguration : IEntityTypeConfiguration<Pagamento>
{
    public void Configure(EntityTypeBuilder<Pagamento> builder)
    {
        builder.ToTable("Pagamentos");

        builder.HasKey(p => p.Id);

        builder.Property(p => p.DataPagamento)
            .IsRequired();

        builder.Property(p => p.ValorPago)
            .IsRequired()
            .HasColumnType("decimal(18,2)");

        builder.Property(p => p.MetodoPagamento)
            .IsRequired();

        builder.Property(p => p.Status)
            .IsRequired();

        builder.HasIndex(p => p.AluguelId)
            .IsUnique();
    }
}
