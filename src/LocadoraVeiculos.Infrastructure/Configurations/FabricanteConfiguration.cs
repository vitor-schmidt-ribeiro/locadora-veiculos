using LocadoraVeiculos.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LocadoraVeiculos.Infrastructure.Configurations;

public class FabricanteConfiguration : IEntityTypeConfiguration<Fabricante>
{
    public void Configure(EntityTypeBuilder<Fabricante> builder)
    {
        builder.ToTable("Fabricantes");

        builder.HasKey(f => f.Id);

        builder.Property(f => f.Nome)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(f => f.PaisOrigem)
            .IsRequired()
            .HasMaxLength(50);

        builder.HasMany(f => f.Veiculos)
            .WithOne(v => v.Fabricante)
            .HasForeignKey(v => v.FabricanteId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
