# Sistema de Locadora de Veículos

Projeto de backend para gerenciamento de locação de veículos desenvolvido em C# com .NET 8, Entity Framework Core e SQL Server Express.

## Estrutura da Solução

O projeto segue a divisão em camadas:

- **LocadoraVeiculos.Domain**: Classes de entidades do domínio e enums.
- **LocadoraVeiculos.Infrastructure**: Configurações de mapeamento relacional (Fluent API), DbContext e migrações do Entity Framework Core.
- **LocadoraVeiculos.Api**: Projeto ASP.NET Core Web API com documentação Swagger.
- **docs**: Documentação do modelo conceitual e script SQL gerado para o banco de dados.

## Entidades Mapeadas

1. **Fabricante**: Cadastro de marcas e fabricantes de veículos.
2. **Categoria**: Classificação dos veículos e definição da diária base.
3. **Veiculo**: Dados do automóvel, incluindo modelo, placa, ano, cor e quilometragem.
4. **Cliente**: Dados cadastrais do cliente com validações de unicidade (CPF, e-mail, CNH).
5. **Aluguel**: Registro do contrato de locação, período, quilometragem inicial e final, valor da diária e valor total.
6. **Pagamento**: Registro do pagamento vinculado ao aluguel.

Para mais detalhes da modelagem e diagrama do banco de dados, consulte o arquivo [docs/MODELO_CONCEITUAL.md](docs/MODELO_CONCEITUAL.md).

## Requisitos e Execução

### Pré-requisitos
- .NET 8 SDK
- SQL Server ou SQL Server Express

### Compilação
```bash
dotnet restore
dotnet build
```

### Configuração do Banco de Dados
A string de conexão está configurada no arquivo `src/LocadoraVeiculos.Api/appsettings.json`:
```json
"ConnectionStrings": {
  "DefaultConnection": "Server=localhost\\SQLEXPRESS;Database=LocadoraVeiculosDb;Trusted_Connection=True;TrustServerCertificate=True;"
}
```

Para aplicar as migrações ao banco de dados:
```bash
dotnet ef database update --project src/LocadoraVeiculos.Infrastructure --startup-project src/LocadoraVeiculos.Api
```

Também é possível criar o esquema executando diretamente o script SQL em `docs/script_banco_sql_express.sql`.

### Executando a API
```bash
dotnet run --project src/LocadoraVeiculos.Api
```
A interface Swagger estará acessível em: `http://localhost:5000/swagger` ou `https://localhost:5001/swagger`.
