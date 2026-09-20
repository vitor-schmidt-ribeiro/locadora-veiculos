using LocadoraVeiculos.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LocadoraVeiculos.Infrastructure.Configurations;

public class VeiculoConfiguration : IEntityTypeConfiguration<Veiculo>
{
    public void Configure(EntityTypeBuilder<Veiculo> builder)
    {
        builder.ToTable("Veiculos");

        builder.HasKey(v => v.Id);

        builder.Property(v => v.Modelo)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(v => v.AnoFabricacao)
            .IsRequired();

        builder.Property(v => v.Quilometragem)
            .IsRequired();

        builder.Property(v => v.Placa)
            .IsRequired()
            .HasMaxLength(10);

        builder.HasIndex(v => v.Placa)
            .IsUnique();

        builder.Property(v => v.Cor)
            .IsRequired()
            .HasMaxLength(30);

        builder.Property(v => v.Status)
            .IsRequired();

        builder.HasOne(v => v.Fabricante)
            .WithMany(f => f.Veiculos)
            .HasForeignKey(v => v.FabricanteId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(v => v.Categoria)
            .WithMany(c => c.Veiculos)
            .HasForeignKey(v => v.CategoriaId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(v => v.Alugueis)
            .WithOne(a => a.Veiculo)
            .HasForeignKey(a => a.VeiculoId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
