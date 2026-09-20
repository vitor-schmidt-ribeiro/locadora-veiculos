using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace LocadoraVeiculos.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Categorias",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nome = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Descricao = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    ValorDiariaBase = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categorias", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Clientes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nome = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    CPF = table.Column<string>(type: "nvarchar(14)", maxLength: 14, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Telefone = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    CNH = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    DataCadastro = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Clientes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Fabricantes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nome = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    PaisOrigem = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Fabricantes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Veiculos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Modelo = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    AnoFabricacao = table.Column<int>(type: "int", nullable: false),
                    Quilometragem = table.Column<int>(type: "int", nullable: false),
                    Placa = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    Cor = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    FabricanteId = table.Column<int>(type: "int", nullable: false),
                    CategoriaId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Veiculos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Veiculos_Categorias_CategoriaId",
                        column: x => x.CategoriaId,
                        principalTable: "Categorias",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Veiculos_Fabricantes_FabricanteId",
                        column: x => x.FabricanteId,
                        principalTable: "Fabricantes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Alugueis",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ClienteId = table.Column<int>(type: "int", nullable: false),
                    VeiculoId = table.Column<int>(type: "int", nullable: false),
                    DataInicio = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DataPrevistaDevolucao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DataDevolucaoEfetiva = table.Column<DateTime>(type: "datetime2", nullable: true),
                    KmInicial = table.Column<int>(type: "int", nullable: false),
                    KmFinal = table.Column<int>(type: "int", nullable: true),
                    ValorDiaria = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ValorTotal = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Alugueis", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Alugueis_Clientes_ClienteId",
                        column: x => x.ClienteId,
                        principalTable: "Clientes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Alugueis_Veiculos_VeiculoId",
                        column: x => x.VeiculoId,
                        principalTable: "Veiculos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Pagamentos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AluguelId = table.Column<int>(type: "int", nullable: false),
                    DataPagamento = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ValorPago = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    MetodoPagamento = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pagamentos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Pagamentos_Alugueis_AluguelId",
                        column: x => x.AluguelId,
                        principalTable: "Alugueis",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Categorias",
                columns: new[] { "Id", "Descricao", "Nome", "ValorDiariaBase" },
                values: new object[,]
                {
                    { 1, "Carros compactos e econômicos, ideais para o dia a dia na cidade.", "Econômico", 110.00m },
                    { 2, "Conforto, amplo porta-malas e excelente dirigibilidade para viagens.", "Sedan Médio", 175.00m },
                    { 3, "Espaço, robustez e versatilidade para toda a família.", "SUV", 240.00m },
                    { 4, "Alto padrão, acabamento sofisticado e máxima performance.", "Premium", 380.00m }
                });

            migrationBuilder.InsertData(
                table: "Clientes",
                columns: new[] { "Id", "CNH", "CPF", "DataCadastro", "Email", "Nome", "Telefone" },
                values: new object[,]
                {
                    { 1, "12345678900", "123.456.789-01", new DateTime(2025, 1, 15, 10, 0, 0, 0, DateTimeKind.Utc), "carlos.silva@email.com", "Carlos Eduardo Silva", "(11) 98765-4321" },
                    { 2, "98765432100", "987.654.321-09", new DateTime(2025, 2, 1, 14, 30, 0, 0, DateTimeKind.Utc), "mariana.santos@email.com", "Mariana Costa Santos", "(21) 99876-5432" },
                    { 3, "45678912300", "456.789.123-45", new DateTime(2025, 2, 20, 9, 15, 0, 0, DateTimeKind.Utc), "roberto.lima@email.com", "Roberto Ferreira Lima", "(31) 97654-3210" }
                });

            migrationBuilder.InsertData(
                table: "Fabricantes",
                columns: new[] { "Id", "Nome", "PaisOrigem" },
                values: new object[,]
                {
                    { 1, "Toyota", "Japão" },
                    { 2, "Volkswagen", "Alemanha" },
                    { 3, "Chevrolet", "Estados Unidos" },
                    { 4, "Fiat", "Itália" },
                    { 5, "Hyundai", "Coreia do Sul" }
                });

            migrationBuilder.InsertData(
                table: "Veiculos",
                columns: new[] { "Id", "AnoFabricacao", "CategoriaId", "Cor", "FabricanteId", "Modelo", "Placa", "Quilometragem", "Status" },
                values: new object[,]
                {
                    { 1, 2023, 2, "Prata", 1, "Corolla XEi 2.0", "BRA2E19", 18500, 1 },
                    { 2, 2024, 1, "Branco", 2, "Polo Track 1.0", "ABC1D23", 8200, 1 },
                    { 3, 2023, 3, "Azul Eclipse", 3, "Tracker Premier 1.2 Turbo", "XYZ9K88", 24000, 2 },
                    { 4, 2022, 1, "Vermelho", 4, "Argo Drive 1.0", "MNO4T56", 35000, 1 },
                    { 5, 2024, 3, "Preto Onix", 5, "Creta Ultimate 2.0", "HYU7B44", 5100, 1 }
                });

            migrationBuilder.InsertData(
                table: "Alugueis",
                columns: new[] { "Id", "ClienteId", "DataDevolucaoEfetiva", "DataInicio", "DataPrevistaDevolucao", "KmFinal", "KmInicial", "Status", "ValorDiaria", "ValorTotal", "VeiculoId" },
                values: new object[,]
                {
                    { 1, 1, new DateTime(2025, 3, 5, 7, 30, 0, 0, DateTimeKind.Utc), new DateTime(2025, 3, 1, 8, 0, 0, 0, DateTimeKind.Utc), new DateTime(2025, 3, 5, 8, 0, 0, 0, DateTimeKind.Utc), 18500, 17900, 2, 175.00m, 700.00m, 1 },
                    { 2, 2, null, new DateTime(2025, 3, 10, 9, 0, 0, 0, DateTimeKind.Utc), new DateTime(2025, 3, 15, 9, 0, 0, 0, DateTimeKind.Utc), null, 24000, 1, 240.00m, 1200.00m, 3 }
                });

            migrationBuilder.InsertData(
                table: "Pagamentos",
                columns: new[] { "Id", "AluguelId", "DataPagamento", "MetodoPagamento", "Status", "ValorPago" },
                values: new object[] { 1, 1, new DateTime(2025, 3, 5, 8, 0, 0, 0, DateTimeKind.Utc), 3, 2, 700.00m });

            migrationBuilder.CreateIndex(
                name: "IX_Alugueis_ClienteId",
                table: "Alugueis",
                column: "ClienteId");

            migrationBuilder.CreateIndex(
                name: "IX_Alugueis_VeiculoId",
                table: "Alugueis",
                column: "VeiculoId");

            migrationBuilder.CreateIndex(
                name: "IX_Categorias_Nome",
                table: "Categorias",
                column: "Nome",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Clientes_CNH",
                table: "Clientes",
                column: "CNH",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Clientes_CPF",
                table: "Clientes",
                column: "CPF",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Clientes_Email",
                table: "Clientes",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Pagamentos_AluguelId",
                table: "Pagamentos",
                column: "AluguelId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Veiculos_CategoriaId",
                table: "Veiculos",
                column: "CategoriaId");

            migrationBuilder.CreateIndex(
                name: "IX_Veiculos_FabricanteId",
                table: "Veiculos",
                column: "FabricanteId");

            migrationBuilder.CreateIndex(
                name: "IX_Veiculos_Placa",
                table: "Veiculos",
                column: "Placa",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Pagamentos");

            migrationBuilder.DropTable(
                name: "Alugueis");

            migrationBuilder.DropTable(
                name: "Clientes");

            migrationBuilder.DropTable(
                name: "Veiculos");

            migrationBuilder.DropTable(
                name: "Categorias");

            migrationBuilder.DropTable(
                name: "Fabricantes");
        }
    }
}
