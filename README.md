# Sistema de Locadora de Veículos

Sistema completo para gerenciamento de locação de veículos composto por uma **API RESTful** desenvolvida em C# com .NET 8 e Entity Framework Core, e uma aplicação **Front-End Single-Page (SPA)** responsiva em HTML5, CSS3, Bootstrap 5 e JavaScript assíncrono.

---

## Estrutura da Solução

O projeto segue arquitetura em camadas desacopladas:

- **`LocadoraVeiculos.Domain`**: Entidades de domínio (`Fabricante`, `Categoria`, `Veiculo`, `Cliente`, `Aluguel`, `Pagamento`) e enums.
- **`LocadoraVeiculos.Infrastructure`**: Mapeamento objeto-relacional (Fluent API), DbContext com carga inicial (*seed*) e migrações do Entity Framework Core.
- **`LocadoraVeiculos.Api`**: Controladores RESTful, DTOs de entrada/saída, tratamento global de exceções (`ErrorHandlingMiddleware`), políticas de CORS e documentação Swagger UI.
- **`frontend/`**: Aplicação cliente SPA (HTML5, Bootstrap 5, Fetch API e CSS responsivo).
- **`docs/`**: Documentação técnica, wireframes vetoriais, artigo SBC em LaTeX e capturas de tela.

---

## Documentação Técnica e Artefatos

- **Wireframes no Figma:** [Acessar Projeto no Figma](https://www.figma.com/design/4RvUybZX26RIYfpTQJoJzv/Trabalho-Pr%C3%A1tico-2?node-id=0-1&t=VaKG7JrY3j0xc9lp-1)
- **Artigo Científico (Padrão SBC):** [docs/artigo_sbc/artigo_sbc.tex](docs/artigo_sbc/artigo_sbc.tex)
- **Modelo Conceitual e DER:** [docs/MODELO_CONCEITUAL.md](docs/MODELO_CONCEITUAL.md)
- **Especificação Completa das APIs:** [docs/DOCUMENTACAO_APIS.md](docs/DOCUMENTACAO_APIS.md)
- **Relatório de Testes:** [docs/RELATORIO_DE_TESTES.md](docs/RELATORIO_DE_TESTES.md)
- **Script SQL para Banco de Dados:** [docs/script_banco_sql_express.sql](docs/script_banco_sql_express.sql)

---

## Como Executar o Projeto

### Pré-requisitos
- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0) instalado.
- Navegador web moderno (Chrome, Edge, Firefox).
- *Opcional:* Python 3 (para executar o servidor estático local do front-end).

---

### Execução no Linux (Ubuntu / Debian / Fedora)

#### 1. Iniciar a API (.NET 8)
Abra um terminal na raiz do repositório:
```bash
# Restaurar dependências e compilar a solução
dotnet build

# Executar a API RESTful
dotnet run --project src/LocadoraVeiculos.Api --launch-profile http
```
- A API iniciará em: `http://localhost:5097`
- Documentação interativa Swagger: `http://localhost:5097/swagger`

#### 2. Iniciar o Front-End
Abra um **segundo terminal** na raiz do projeto:

**Opção A — Servidor HTTP local (Recomendado):**
```bash
python3 -m http.server 3000 --directory frontend
```
Acesse no navegador: **`http://localhost:3000`**

**Opção B — Abrir diretamente no navegador padrão:**
```bash
xdg-open frontend/index.html
```

---

### Execução no Windows (PowerShell ou Prompt de Comando CMD)

#### 1. Iniciar a API (.NET 8)
Abra o PowerShell ou o CMD na pasta do repositório:
```powershell
# Restaurar dependências e compilar a solução
dotnet build

# Executar a API RESTful
dotnet run --project src\LocadoraVeiculos.Api --launch-profile http
```
- A API iniciará em: `http://localhost:5097`
- Documentação interativa Swagger: `http://localhost:5097/swagger`

#### 2. Iniciar o Front-End
Abra uma **segunda janela** do PowerShell ou CMD na pasta do projeto:

**Opção A — Servidor HTTP local com Python (Recomendado):**
```powershell
python -m http.server 3000 --directory frontend
```
Acesse no navegador: **`http://localhost:3000`**

**Opção B — Abrir diretamente no navegador padrão (Sem Python):**
```powershell
# No PowerShell ou CMD:
start frontend\index.html
```

---

## Endpoints da API

### Operações CRUD

#### 1. Fabricantes (`/api/Fabricantes`)
- `GET /api/Fabricantes`: Lista todos os fabricantes.
- `GET /api/Fabricantes/{id}`: Consulta fabricante por ID.
- `POST /api/Fabricantes`: Cadastra novo fabricante.
- `PUT /api/Fabricantes/{id}`: Atualiza dados do fabricante.
- `DELETE /api/Fabricantes/{id}`: Remove fabricante (com integridade referencial).

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
- `DELETE /api/Veiculos/{id}`: Remove veículo (com verificação de histórico).

#### 4. Clientes (`/api/Clientes`)
- `GET /api/Clientes`: Lista todos os clientes cadastrados.
- `GET /api/Clientes/{id}`: Consulta cliente por ID.
- `POST /api/Clientes`: Cadastra cliente (validação de CPF, e-mail e CNH únicos).
- `PUT /api/Clientes/{id}`: Atualiza dados cadastrais.
- `DELETE /api/Clientes/{id}`: Remove cliente (com verificação de contratos).

#### 5. Aluguéis (`/api/Alugueis`)
- `GET /api/Alugueis`: Lista todos os contratos de locação.
- `GET /api/Alugueis/{id}`: Consulta aluguel por ID.
- `POST /api/Alugueis`: Abre novo contrato de locação (altera veículo para Alugado).
- `PUT /api/Alugueis/{id}/devolucao`: Registra a devolução (recalcula valor e odômetro).
- `DELETE /api/Alugueis/{id}`: Cancela contrato ativo.

---

### Consultas com Junções Relacionais (Joins)

1. **Filtro 1 - Veículos Disponíveis por Categoria e Fabricante (`INNER JOIN`)**
   - Rota: `GET /api/Veiculos/disponiveis?categoriaId={id}&fabricanteId={id}`
   - Junção: `INNER JOIN` entre `Veiculos`, `Categorias` e `Fabricantes`.

2. **Filtro 2 - Histórico de Locações do Cliente (`INNER JOIN`)**
   - Rota: `GET /api/Alugueis/cliente/{clienteId}?status={status}`
   - Junção: `INNER JOIN` entre `Alugueis`, `Clientes`, `Veiculos` e `Fabricantes`.

3. **Filtro 3 - Relatório de Clientes e Locações (`LEFT JOIN`)**
   - Rota: `GET /api/Clientes/relatorio-locacoes`
   - Junção: `LEFT JOIN` entre `Clientes` e `Alugueis` (lista todos os clientes cadastrados).

4. **Filtro 4 - Relatório de Frota e Desempenho (`LEFT JOIN` e `INNER JOIN`)**
   - Rota: `GET /api/Veiculos/relatorio-frota`
   - Junção: `LEFT JOIN` entre `Veiculos` e `Alugueis`, com `INNER JOIN` em `Fabricantes` e `Categorias`.

5. **Filtro 5 - Situação Financeira dos Aluguéis (`LEFT JOIN` e `INNER JOIN`)**
   - Rota: `GET /api/Alugueis/status-pagamento?statusPagamento={status}`
   - Junção: `LEFT JOIN` entre `Alugueis` e `Pagamentos`, com `INNER JOIN` em `Clientes` e `Veiculos`.

---

## Funcionalidades do Front-End (TP2)

- **Dashboard Executivo:** Indicadores em tempo real (Total de Clientes, Frota, Aluguéis Ativos e Faturamento).
- **CRUDs Completos:** Telas e modais para Fabricantes, Categorias, Veículos, Clientes e Aluguéis/Devoluções.
- **Filtros em Múltiplos Campos:** Pesquisa dinâmica por Nome e CPF em Clientes; Modelo, Placa e Categoria em Veículos; Cliente e Status em Aluguéis.
- **Validações com Máscaras em Tempo Real:** Formatação instantânea de CPF (`000.000.000-00`) e Telefone (`(00) 00000-0000`) com limites máximos de caracteres.
- **Prevenção de Duplo Envio:** Desabilitação com indicador de carregamento (*spinner*) em todos os formulários.
- **Painel de Consultas Avançadas:** Visualização em abas com execução parametrizada das consultas com `INNER JOIN` e `LEFT JOIN`.

---

## Apresentação do Trabalho

- **Vídeo de Apresentação (Pitch de 3 a 5 Minutos):** `[Inserir o link do vídeo gravado (YouTube / Google Drive)]`
