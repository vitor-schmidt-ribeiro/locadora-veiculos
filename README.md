# Sistema de Locadora de Veículos

Projeto de backend para gerenciamento de locação de veículos desenvolvido em C# com .NET 8, ASP.NET Core Web API, Entity Framework Core e SQL Server Express.

## Estrutura da Solução

O projeto segue a divisão em camadas:

- **LocadoraVeiculos.Domain**: Classes de entidades de domínio e enums.
- **LocadoraVeiculos.Infrastructure**: Mapeamento relacional (Fluent API), DbContext com carga inicial (seed) e migrações do Entity Framework Core.
- **LocadoraVeiculos.Api**: Controladores RESTful, DTOs de entrada e saída, middleware de tratamento de erros e documentação Swagger.
- **docs**: Documentação do modelo conceitual (DER e dicionário de dados) e script SQL DDL.

## Endpoints da API

### Operações CRUD

#### 1. Fabricantes (`/api/fabricantes`)
- `GET /api/fabricantes`: Lista todos os fabricantes.
- `GET /api/fabricantes/{id}`: Consulta fabricante por ID.
- `POST /api/fabricantes`: Cadastra novo fabricante.
- `PUT /api/fabricantes/{id}`: Atualiza dados do fabricante.
- `DELETE /api/fabricantes/{id}`: Remove fabricante (com validação de integridade referencial).

#### 2. Categorias (`/api/categorias`)
- `GET /api/categorias`: Lista todas as categorias.
- `GET /api/categorias/{id}`: Consulta categoria por ID.
- `POST /api/categorias`: Cadastra nova categoria com diária base.
- `PUT /api/categorias/{id}`: Atualiza categoria.
- `DELETE /api/categorias/{id}`: Remove categoria (com validação de veículos vinculados).

#### 3. Veículos (`/api/veiculos`)
- `GET /api/veiculos`: Lista todos os veículos com fabricante e categoria.
- `GET /api/veiculos/{id}`: Consulta veículo por ID.
- `POST /api/veiculos`: Cadastra veículo (com validação de placa única e chaves estrangeiras).
- `PUT /api/veiculos/{id}`: Atualiza dados do veículo.
- `DELETE /api/veiculos/{id}`: Remove veículo (com verificação de histórico de locações).

#### 4. Clientes (`/api/clientes`)
- `GET /api/clientes`: Lista todos os clientes.
- `GET /api/clientes/{id}`: Consulta cliente por ID.
- `POST /api/clientes`: Cadastra cliente (com validação de unicidade de CPF, e-mail e CNH).
- `PUT /api/clientes/{id}`: Atualiza dados do cliente.
- `DELETE /api/clientes/{id}`: Remove cliente (com verificação de contratos).

#### 5. Aluguéis (`/api/alugueis`)
- `GET /api/alugueis`: Lista todos os aluguéis registrados.
- `GET /api/alugueis/{id}`: Consulta aluguel por ID.
- `POST /api/alugueis`: Cria novo contrato de locação (valida disponibilidade do veículo, calcula previsão financeira e atualiza status do veículo para Alugado).
- `PUT /api/alugueis/{id}/devolucao`: Registra a devolução do veículo (valida quilometragem final, recalcula valor final e libera o veículo para Disponível).
- `DELETE /api/alugueis/{id}`: Cancela contrato ativo ou remove histórico.

---

### Filtros com Junções (Joins)

O sistema possui 5 rotas de filtros especializadas que utilizam diferentes tipos de junção relacional:

1. **Filtro 1 - Veículos Disponíveis por Categoria e Fabricante (`INNER JOIN`)**
   - Rota: `GET /api/veiculos/disponiveis?categoriaId={id}&fabricanteId={id}`
   - Junção: `INNER JOIN` entre `Veiculos`, `Categorias` e `Fabricantes`.
   - Finalidade: Permite localizar automóveis prontos para locação filtrados por porte e marca.

2. **Filtro 2 - Histórico de Locações do Cliente (`INNER JOIN`)**
   - Rota: `GET /api/alugueis/cliente/{clienteId}?status={status}`
   - Junção: `INNER JOIN` entre `Alugueis`, `Clientes`, `Veiculos` e `Fabricantes`.
   - Finalidade: Consulta todos os contratos de um cliente com os dados completos do veículo locado e filtro opcional por status do contrato.

3. **Filtro 3 - Relatório de Clientes e Locações (`LEFT JOIN`)**
   - Rota: `GET /api/clientes/relatorio-locacoes`
   - Junção: `LEFT JOIN` entre `Clientes` e `Alugueis`.
   - Finalidade: Lista todos os clientes cadastrados com total de locações e valor gasto acumulado, incluindo clientes que nunca realizaram aluguel.

4. **Filtro 4 - Relatório de Frota e Desempenho (`LEFT JOIN` e `INNER JOIN`)**
   - Rota: `GET /api/veiculos/relatorio-frota`
   - Junção: `LEFT JOIN` entre `Veiculos` e `Alugueis`, combinado com `INNER JOIN` em `Fabricantes` e `Categorias`.
   - Finalidade: Exibe a frota com quantidade de locações, faturamento total gerado e último odômetro registrado, abrangendo também veículos sem histórico de locação.

5. **Filtro 5 - Situação Financeira dos Aluguéis (`LEFT JOIN` e `INNER JOIN`)**
   - Rota: `GET /api/alugueis/status-pagamento?statusPagamento={status}`
   - Junção: `LEFT JOIN` entre `Alugueis` e `Pagamentos`, com `INNER JOIN` em `Clientes` e `Veiculos`.
   - Finalidade: Permite filtrar contratos por status de pagamento (pendente, pago ou sem registro de liquidação).

---

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
A string de conexão padrão encontra-se em `src/LocadoraVeiculos.Api/appsettings.json`:
```json
"ConnectionStrings": {
  "DefaultConnection": "Server=localhost\\SQLEXPRESS;Database=LocadoraVeiculosDb;Trusted_Connection=True;TrustServerCertificate=True;"
}
```

Para aplicar as migrações ao banco:
```bash
dotnet ef database update --project src/LocadoraVeiculos.Infrastructure --startup-project src/LocadoraVeiculos.Api
```

Ou execute o script SQL em `docs/script_banco_sql_express.sql`.

### Execução da API
```bash
dotnet run --project src/LocadoraVeiculos.Api
```
O Swagger estará acessível em: `http://localhost:5000/swagger` ou `https://localhost:5001/swagger`.
