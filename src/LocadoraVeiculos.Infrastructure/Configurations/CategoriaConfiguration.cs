using LocadoraVeiculos.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LocadoraVeiculos.Infrastructure.Configurations;

public class CategoriaConfiguration : IEntityTypeConfiguration<Categoria>
{
    public void Configure(EntityTypeBuilder<Categoria> builder)
    {
        builder.ToTable("Categorias");

        builder.HasKey(c => c.Id);

        builder.Property(c => c.Nome)
            .IsRequired()
            .HasMaxLength(50);

        builder.HasIndex(c => c.Nome)
            .IsUnique();

        builder.Property(c => c.Descricao)
            .HasMaxLength(250)
            .IsRequired(false);

        builder.Property(c => c.ValorDiariaBase)
            .IsRequired()
            .HasColumnType("decimal(18,2)");

        builder.HasMany(c => c.Veiculos)
            .WithOne(v => v.Categoria)
            .HasForeignKey(v => v.CategoriaId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
