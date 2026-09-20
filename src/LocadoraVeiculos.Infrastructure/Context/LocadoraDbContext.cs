using LocadoraVeiculos.Domain.Entities;
using LocadoraVeiculos.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace LocadoraVeiculos.Infrastructure.Context;

public class LocadoraDbContext : DbContext
{
    public LocadoraDbContext(DbContextOptions<LocadoraDbContext> options) : base(options)
    {
    }

    public DbSet<Fabricante> Fabricantes => Set<Fabricante>();
    public DbSet<Categoria> Categorias => Set<Categoria>();
    public DbSet<Veiculo> Veiculos => Set<Veiculo>();
    public DbSet<Cliente> Clientes => Set<Cliente>();
    public DbSet<Aluguel> Alugueis => Set<Aluguel>();
    public DbSet<Pagamento> Pagamentos => Set<Pagamento>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(LocadoraDbContext).Assembly);

        SeedInitialData(modelBuilder);
    }

    private static void SeedInitialData(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Fabricante>().HasData(
            new Fabricante { Id = 1, Nome = "Toyota", PaisOrigem = "Japão" },
            new Fabricante { Id = 2, Nome = "Volkswagen", PaisOrigem = "Alemanha" },
            new Fabricante { Id = 3, Nome = "Chevrolet", PaisOrigem = "Estados Unidos" },
            new Fabricante { Id = 4, Nome = "Fiat", PaisOrigem = "Itália" },
            new Fabricante { Id = 5, Nome = "Hyundai", PaisOrigem = "Coreia do Sul" }
        );

        modelBuilder.Entity<Categoria>().HasData(
            new Categoria { Id = 1, Nome = "Econômico", Descricao = "Carros compactos e econômicos para cidade", ValorDiariaBase = 110.00m },
            new Categoria { Id = 2, Nome = "Sedan Médio", Descricao = "Sedans médios confortáveis para viagens", ValorDiariaBase = 175.00m },
            new Categoria { Id = 3, Nome = "SUV", Descricao = "Veículos utilitários esportivos espaçosos", ValorDiariaBase = 240.00m },
            new Categoria { Id = 4, Nome = "Premium", Descricao = "Veículos de luxo e alta performance", ValorDiariaBase = 380.00m }
        );

        modelBuilder.Entity<Veiculo>().HasData(
            new Veiculo
            {
                Id = 1,
                FabricanteId = 1,
                CategoriaId = 2,
                Modelo = "Corolla XEi 2.0",
                AnoFabricacao = 2023,
                Placa = "BRA2E19",
                Cor = "Prata",
                Quilometragem = 18500,
                Status = StatusVeiculo.Disponivel
            },
            new Veiculo
            {
                Id = 2,
                FabricanteId = 2,
                CategoriaId = 1,
                Modelo = "Polo Track 1.0",
                AnoFabricacao = 2024,
                Placa = "ABC1D23",
                Cor = "Branco",
                Quilometragem = 8200,
                Status = StatusVeiculo.Disponivel
            },
            new Veiculo
            {
                Id = 3,
                FabricanteId = 3,
                CategoriaId = 3,
                Modelo = "Tracker Premier 1.2 Turbo",
                AnoFabricacao = 2023,
                Placa = "XYZ9K88",
                Cor = "Azul",
                Quilometragem = 24000,
                Status = StatusVeiculo.Alugado
            },
            new Veiculo
            {
                Id = 4,
                FabricanteId = 4,
                CategoriaId = 1,
                Modelo = "Argo Drive 1.0",
                AnoFabricacao = 2022,
                Placa = "MNO4T56",
                Cor = "Vermelho",
                Quilometragem = 35000,
                Status = StatusVeiculo.Disponivel
            },
            new Veiculo
            {
                Id = 5,
                FabricanteId = 5,
                CategoriaId = 3,
                Modelo = "Creta Ultimate 2.0",
                AnoFabricacao = 2024,
                Placa = "HYU7B44",
                Cor = "Preto",
                Quilometragem = 5100,
                Status = StatusVeiculo.Disponivel
            }
        );

        modelBuilder.Entity<Cliente>().HasData(
            new Cliente
            {
                Id = 1,
                Nome = "Carlos Eduardo Silva",
                CPF = "123.456.789-01",
                Email = "carlos.silva@email.com",
                Telefone = "(11) 98765-4321",
                CNH = "12345678900",
                DataCadastro = new DateTime(2025, 1, 15, 10, 0, 0, DateTimeKind.Utc)
            },
            new Cliente
            {
                Id = 2,
                Nome = "Mariana Costa Santos",
                CPF = "987.654.321-09",
                Email = "mariana.santos@email.com",
                Telefone = "(21) 99876-5432",
                CNH = "98765432100",
                DataCadastro = new DateTime(2025, 2, 1, 14, 30, 0, DateTimeKind.Utc)
            },
            new Cliente
            {
                Id = 3,
                Nome = "Roberto Ferreira Lima",
                CPF = "456.789.123-45",
                Email = "roberto.lima@email.com",
                Telefone = "(31) 97654-3210",
                CNH = "45678912300",
                DataCadastro = new DateTime(2025, 2, 20, 9, 15, 0, DateTimeKind.Utc)
            }
        );

        modelBuilder.Entity<Aluguel>().HasData(
            new Aluguel
            {
                Id = 1,
                ClienteId = 1,
                VeiculoId = 1,
                DataInicio = new DateTime(2025, 3, 1, 8, 0, 0, DateTimeKind.Utc),
                DataPrevistaDevolucao = new DateTime(2025, 3, 5, 8, 0, 0, DateTimeKind.Utc),
                DataDevolucaoEfetiva = new DateTime(2025, 3, 5, 7, 30, 0, DateTimeKind.Utc),
                KmInicial = 17900,
                KmFinal = 18500,
                ValorDiaria = 175.00m,
                ValorTotal = 700.00m,
                Status = StatusAluguel.Concluido
            },
            new Aluguel
            {
                Id = 2,
                ClienteId = 2,
                VeiculoId = 3,
                DataInicio = new DateTime(2025, 3, 10, 9, 0, 0, DateTimeKind.Utc),
                DataPrevistaDevolucao = new DateTime(2025, 3, 15, 9, 0, 0, DateTimeKind.Utc),
                DataDevolucaoEfetiva = null,
                KmInicial = 24000,
                KmFinal = null,
                ValorDiaria = 240.00m,
                ValorTotal = 1200.00m,
                Status = StatusAluguel.Ativo
            }
        );

        modelBuilder.Entity<Pagamento>().HasData(
            new Pagamento
            {
                Id = 1,
                AluguelId = 1,
                DataPagamento = new DateTime(2025, 3, 5, 8, 0, 0, DateTimeKind.Utc),
                ValorPago = 700.00m,
                MetodoPagamento = MetodoPagamento.Pix,
                Status = StatusPagamento.Pago
            }
        );
    }
}
