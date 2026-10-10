# Sistema de Locadora de Veículos

Projeto de backend para gerenciamento de locação de veículos desenvolvido em C# com .NET 8, ASP.NET Core Web API, Entity Framework Core e SQL Server Express.

## Estrutura da Solução

O projeto segue a divisão em camadas:

- **LocadoraVeiculos.Domain**: Classes de entidades de domínio e enums.
- **LocadoraVeiculos.Infrastructure**: Mapeamento relacional (Fluent API), DbContext com carga inicial (seed) e migrações do Entity Framework Core.
- **LocadoraVeiculos.Api**: Controladores RESTful, DTOs de entrada e saída, middleware de tratamento de erros e documentação interativa via Swagger com anotações XML.
- **docs**: Documentação técnica do modelo conceitual (DER e dicionário de dados), especificação formal das APIs, relatório de testes com prints de tela e script SQL DDL.

---

## Documentação Técnica e Relatórios

- **Modelo Conceitual e DER:** [docs/MODELO_CONCEITUAL.md](docs/MODELO_CONCEITUAL.md)
- **Especificação Completa das APIs:** [docs/DOCUMENTACAO_APIS.md](docs/DOCUMENTACAO_APIS.md)
- **Relatório de Testes:** [docs/RELATORIO_DE_TESTES.md](docs/RELATORIO_DE_TESTES.md)
- **Script SQL para Banco de Dados:** [docs/script_banco_sql_express.sql](docs/script_banco_sql_express.sql)
- **Evidências do Swagger UI:** [docs/screenshots/](docs/screenshots/)

---

## Endpoints da API

### Operações CRUD

#### 1. Fabricantes (`/api/Fabricantes`)
- `GET /api/Fabricantes`: Lista todos os fabricantes.
- `GET /api/Fabricantes/{id}`: Consulta fabricante por ID.
- `POST /api/Fabricantes`: Cadastra novo fabricante.
- `PUT /api/Fabricantes/{id}`: Atualiza dados do fabricante.
- `DELETE /api/Fabricantes/{id}`: Remove fabricante (com validação de integridade referencial).

#### 2. Categorias (`/api/Categorias`)
- `GET /api/Categorias`: Lista todas as categorias com diárias base.
- `GET /api/Categorias/{id}`: Consulta categoria por ID.
- `POST /api/Categorias`: Cadastra nova categoria.
- `PUT /api/Categorias/{id}`: Atualiza categoria.
- `DELETE /api/Categorias/{id}`: Remove categoria (com validação de veículos vinculados).

#### 3. Veículos (`/api/Veiculos`)
- `GET /api/Veiculos`: Lista todos os veículos da frota.
- `GET /api/Veiculos/{id}`: Consulta veículo por ID.
- `POST /api/Veiculos`: Cadastra veículo (com validação de placa única).
- `PUT /api/Veiculos/{id}`: Atualiza dados do veículo.
- `DELETE /api/Veiculos/{id}`: Remove veículo (com verificação de histórico de locações).

#### 4. Clientes (`/api/Clientes`)
- `GET /api/Clientes`: Lista todos os clientes cadastrados.
- `GET /api/Clientes/{id}`: Consulta cliente por ID.
- `POST /api/Clientes`: Cadastra cliente (com validação de unicidade de CPF, e-mail e CNH).
- `PUT /api/Clientes/{id}`: Atualiza dados cadastrais.
- `DELETE /api/Clientes/{id}`: Remove cliente (com verificação de contratos).

#### 5. Aluguéis (`/api/Alugueis`)
- `GET /api/Alugueis`: Lista todos os contratos de locação.
- `GET /api/Alugueis/{id}`: Consulta aluguel por ID.
- `POST /api/Alugueis`: Abre novo contrato de locação (valida disponibilidade e altera status para Alugado).
- `PUT /api/Alugueis/{id}/devolucao`: Registra a devolução do veículo (valida odômetro, calcula valor final e libera o veículo).
- `DELETE /api/Alugueis/{id}`: Cancela contrato ativo ou remove histórico.

---

### Filtros com Junções (Joins)

1. **Filtro 1 - Veículos Disponíveis por Categoria e Fabricante (`INNER JOIN`)**
   - Rota: `GET /api/Veiculos/disponiveis?categoriaId={id}&fabricanteId={id}`
   - Junção: `INNER JOIN` entre `Veiculos`, `Categorias` e `Fabricantes`.

2. **Filtro 2 - Histórico de Locações do Cliente (`INNER JOIN`)**
   - Rota: `GET /api/Alugueis/cliente/{clienteId}?status={status}`
   - Junção: `INNER JOIN` entre `Alugueis`, `Clientes`, `Veiculos` e `Fabricantes`.

3. **Filtro 3 - Relatório de Clientes e Locações (`LEFT JOIN`)**
   - Rota: `GET /api/Clientes/relatorio-locacoes`
   - Junção: `LEFT JOIN` entre `Clientes` e `Alugueis`.

4. **Filtro 4 - Relatório de Frota e Desempenho (`LEFT JOIN` e `INNER JOIN`)**
   - Rota: `GET /api/Veiculos/relatorio-frota`
   - Junção: `LEFT JOIN` entre `Veiculos` e `Alugueis`, combinado com `INNER JOIN` em `Fabricantes` e `Categorias`.

5. **Filtro 5 - Situação Financeira dos Aluguéis (`LEFT JOIN` e `INNER JOIN`)**
   - Rota: `GET /api/Alugueis/status-pagamento?statusPagamento={status}`
   - Junção: `LEFT JOIN` entre `Alugueis` e `Pagamentos`, com `INNER JOIN` em `Clientes` e `Veiculos`.

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

---

## Etapa 4 - Apresentação do Projeto (Vídeo)

- **Link do Vídeo da Apresentação:** `[Inserir o link do vídeo gravado (YouTube / Google Drive / Teams)]`
