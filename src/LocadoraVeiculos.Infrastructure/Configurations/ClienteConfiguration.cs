using LocadoraVeiculos.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LocadoraVeiculos.Infrastructure.Configurations;

public class ClienteConfiguration : IEntityTypeConfiguration<Cliente>
{
    public void Configure(EntityTypeBuilder<Cliente> builder)
    {
        builder.ToTable("Clientes");

        builder.HasKey(c => c.Id);

        builder.Property(c => c.Nome)
            .IsRequired()
            .HasMaxLength(150);

        builder.Property(c => c.CPF)
            .IsRequired()
            .HasMaxLength(14);

        builder.HasIndex(c => c.CPF)
            .IsUnique();

        builder.Property(c => c.Email)
            .IsRequired()
            .HasMaxLength(100);

        builder.HasIndex(c => c.Email)
            .IsUnique();

        builder.Property(c => c.Telefone)
            .IsRequired()
            .HasMaxLength(20);

        builder.Property(c => c.CNH)
            .IsRequired()
            .HasMaxLength(20);

        builder.HasIndex(c => c.CNH)
            .IsUnique();

        builder.Property(c => c.DataCadastro)
            .IsRequired();

        builder.HasMany(c => c.Alugueis)
            .WithOne(a => a.Cliente)
            .HasForeignKey(a => a.ClienteId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
