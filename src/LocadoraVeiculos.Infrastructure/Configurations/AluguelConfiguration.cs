using LocadoraVeiculos.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LocadoraVeiculos.Infrastructure.Configurations;

public class AluguelConfiguration : IEntityTypeConfiguration<Aluguel>
{
    public void Configure(EntityTypeBuilder<Aluguel> builder)
    {
        builder.ToTable("Alugueis");

        builder.HasKey(a => a.Id);

        builder.Property(a => a.DataInicio)
            .IsRequired();

        builder.Property(a => a.DataPrevistaDevolucao)
            .IsRequired();

        builder.Property(a => a.DataDevolucaoEfetiva)
            .IsRequired(false);

        builder.Property(a => a.KmInicial)
            .IsRequired();

        builder.Property(a => a.KmFinal)
            .IsRequired(false);

        builder.Property(a => a.ValorDiaria)
            .IsRequired()
            .HasColumnType("decimal(18,2)");

        builder.Property(a => a.ValorTotal)
            .IsRequired()
            .HasColumnType("decimal(18,2)");

        builder.Property(a => a.Status)
            .IsRequired();

        builder.HasOne(a => a.Cliente)
            .WithMany(c => c.Alugueis)
            .HasForeignKey(a => a.ClienteId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(a => a.Veiculo)
            .WithMany(v => v.Alugueis)
            .HasForeignKey(a => a.VeiculoId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(a => a.Pagamento)
            .WithOne(p => p.Aluguel)
            .HasForeignKey<Pagamento>(p => p.AluguelId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
