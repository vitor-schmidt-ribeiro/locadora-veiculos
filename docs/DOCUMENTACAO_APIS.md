# Documentação das APIs RESTful
## Sistema de Locadora de Veículos

---

### 1. Visão Geral

Este documento apresenta a especificação técnica de todos os endpoints RESTful desenvolvidos para o Sistema de Locadora de Veículos. A API foi implementada em C# sobre a plataforma ASP.NET Core (.NET 8), utilizando Entity Framework Core para persistência relacional com o Microsoft SQL Server.

A interface interativa de testes e documentação aberta (OpenAPI) está disponível via Swagger no endpoint:
`http://localhost:5000/swagger` ou `https://localhost:5001/swagger`.

#### Padrões de Comunicação
- **Formato dos Dados:** JSON (`application/json`)
- **Codificação:** UTF-8
- **Códigos de Status HTTP Padronizados:**
  - `200 OK`: Requisição processada com sucesso retornando dados.
  - `201 Created`: Novo registro criado com sucesso (retorna cabeçalho `Location`).
  - `204 No Content`: Operação executada com sucesso sem conteúdo no corpo de resposta (típico de updates e deletes).
  - `400 Bad Request`: Dados de entrada inválidos ou violação de regras de negócio.
  - `404 Not Found`: Recurso não localizado para o identificador fornecido.
  - `409 Conflict`: Conflito de integridade (ex.: duplicidade de chave única como CPF, Placa ou CNH).
  - `500 Internal Server Error`: Erro interno no servidor, capturado pelo middleware global de exceções.

---

### 2. Módulo de Fabricantes (`/api/fabricantes`)

#### 2.1. Listar Fabricantes
- **Método HTTP:** `GET`
- **Rota:** `/api/fabricantes`
- **Descrição:** Retorna a coleção de todas as marcas cadastradas.
- **Parâmetros:** Nenhum.
- **Códigos de Resposta:** `200 OK`
- **Exemplo de Resposta (200 OK):**
```json
[
  {
    "id": 1,
    "nome": "Toyota",
    "paisOrigem": "Japão"
  },
  {
    "id": 2,
    "nome": "Volkswagen",
    "paisOrigem": "Alemanha"
  }
]
```

#### 2.2. Obter Fabricante por ID
- **Método HTTP:** `GET`
- **Rota:** `/api/fabricantes/{id}`
- **Descrição:** Busca os dados de um fabricante pelo identificador numérico.
- **Parâmetros de Rota:** `id` (int, obrigatório)
- **Códigos de Resposta:** `200 OK`, `404 Not Found`
- **Exemplo de Resposta (200 OK):**
```json
{
  "id": 1,
  "nome": "Toyota",
  "paisOrigem": "Japão"
}
```
- **Exemplo de Resposta (404 Not Found):**
```json
{
  "mensagem": "Fabricante com ID 99 não encontrado."
}
```

#### 2.3. Cadastrar Fabricante
- **Método HTTP:** `POST`
- **Rota:** `/api/fabricantes`
- **Descrição:** Cria um novo registro de fabricante.
- **Corpo da Requisição (JSON):**
```json
{
  "nome": "Honda",
  "paisOrigem": "Japão"
}
```
- **Códigos de Resposta:** `201 Created`, `400 Bad Request`
- **Exemplo de Resposta (201 Created):**
```json
{
  "id": 6,
  "nome": "Honda",
  "paisOrigem": "Japão"
}
```

#### 2.4. Atualizar Fabricante
- **Método HTTP:** `PUT`
- **Rota:** `/api/fabricantes/{id}`
- **Descrição:** Atualiza os dados de um fabricante existente.
- **Parâmetros de Rota:** `id` (int, obrigatório)
- **Corpo da Requisição (JSON):**
```json
{
  "nome": "Honda Motors",
  "paisOrigem": "Japão"
}
```
- **Códigos de Resposta:** `204 No Content`, `400 Bad Request`, `404 Not Found`

#### 2.5. Excluir Fabricante
- **Método HTTP:** `DELETE`
- **Rota:** `/api/fabricantes/{id}`
- **Descrição:** Remove o fabricante do banco de dados (bloqueado se houver veículos associados).
- **Parâmetros de Rota:** `id` (int, obrigatório)
- **Códigos de Resposta:** `204 No Content`, `400 Bad Request`, `404 Not Found`
- **Exemplo de Resposta (400 Bad Request):**
```json
{
  "mensagem": "Não é possível excluir o fabricante pois existem veículos vinculados a ele."
}
```

---

### 3. Módulo de Categorias (`/api/categorias`)

#### 3.1. Listar Categorias
- **Método HTTP:** `GET`
- **Rota:** `/api/categorias`
- **Descrição:** Lista as categorias com descrições e tarifas de diária base.
- **Códigos de Resposta:** `200 OK`
- **Exemplo de Resposta (200 OK):**
```json
[
  {
    "id": 1,
    "nome": "Econômico",
    "descricao": "Carros compactos e econômicos para cidade",
    "valorDiariaBase": 110.00
  },
  {
    "id": 3,
    "nome": "SUV",
    "descricao": "Veículos utilitários esportivos espaçosos",
    "valorDiariaBase": 240.00
  }
]
```

#### 3.2. Obter Categoria por ID
- **Método HTTP:** `GET`
- **Rota:** `/api/categorias/{id}`
- **Códigos de Resposta:** `200 OK`, `404 Not Found`

#### 3.3. Cadastrar Categoria
- **Método HTTP:** `POST`
- **Rota:** `/api/categorias`
- **Corpo da Requisição (JSON):**
```json
{
  "nome": "Pick-up",
  "descricao": "Caminhonetes cabine dupla para carga e tração",
  "valorDiariaBase": 320.00
}
```
- **Códigos de Resposta:** `201 Created`, `400 Bad Request`, `409 Conflict`

#### 3.4. Atualizar Categoria
- **Método HTTP:** `PUT`
- **Rota:** `/api/categorias/{id}`
- **Códigos de Resposta:** `204 No Content`, `400 Bad Request`, `404 Not Found`, `409 Conflict`

#### 3.5. Excluir Categoria
- **Método HTTP:** `DELETE`
- **Rota:** `/api/categorias/{id}`
- **Códigos de Resposta:** `204 No Content`, `400 Bad Request`, `404 Not Found`

---

### 4. Módulo de Veículos (`/api/veiculos`)

#### 4.1. Listar Veículos
- **Método HTTP:** `GET`
- **Rota:** `/api/veiculos`
- **Descrição:** Lista todos os automóveis cadastrados com os nomes do fabricante e da categoria associados.
- **Códigos de Resposta:** `200 OK`
- **Exemplo de Resposta (200 OK):**
```json
[
  {
    "id": 1,
    "modelo": "Corolla XEi 2.0",
    "anoFabricacao": 2023,
    "quilometragem": 18500,
    "placa": "BRA2E19",
    "cor": "Prata",
    "status": "Disponivel",
    "fabricanteId": 1,
    "fabricanteNome": "Toyota",
    "categoriaId": 2,
    "categoriaNome": "Sedan Médio"
  }
]
```

#### 4.2. Obter Veículo por ID
- **Método HTTP:** `GET`
- **Rota:** `/api/veiculos/{id}`
- **Códigos de Resposta:** `200 OK`, `404 Not Found`

#### 4.3. Cadastrar Veículo
- **Método HTTP:** `POST`
- **Rota:** `/api/veiculos`
- **Descrição:** Insere um novo veículo na frota validando unicidade da placa e existência das chaves estrangeiras.
- **Corpo da Requisição (JSON):**
```json
{
  "modelo": "Civic Touring 1.5",
  "anoFabricacao": 2023,
  "quilometragem": 12000,
  "placa": "CIV3C33",
  "cor": "Branco Perolizado",
  "fabricanteId": 1,
  "categoriaId": 2
}
```
- **Códigos de Resposta:** `201 Created`, `400 Bad Request`, `409 Conflict`

#### 4.4. Atualizar Veículo
- **Método HTTP:** `PUT`
- **Rota:** `/api/veiculos/{id}`
- **Corpo da Requisição (JSON):**
```json
{
  "modelo": "Civic Touring 1.5 Turbo",
  "anoFabricacao": 2023,
  "quilometragem": 13500,
  "placa": "CIV3C33",
  "cor": "Branco Perolizado",
  "status": "Disponivel",
  "fabricanteId": 1,
  "categoriaId": 2
}
```
- **Códigos de Resposta:** `204 No Content`, `400 Bad Request`, `404 Not Found`, `409 Conflict`

#### 4.5. Excluir Veículo
- **Método HTTP:** `DELETE`
- **Rota:** `/api/veiculos/{id}`
- **Códigos de Resposta:** `204 No Content`, `400 Bad Request`, `404 Not Found`

#### 4.6. Filtro 1: Veículos Disponíveis por Categoria e Fabricante (`INNER JOIN`)
- **Método HTTP:** `GET`
- **Rota:** `/api/veiculos/disponiveis?categoriaId={id}&fabricanteId={id}`
- **Descrição:** Realiza `INNER JOIN` entre `Veiculos`, `Categorias` e `Fabricantes`, filtrando apenas veículos com status `Disponivel`.
- **Parâmetros de Consulta (Query):**
  - `categoriaId` (int, opcional)
  - `fabricanteId` (int, opcional)
- **Códigos de Resposta:** `200 OK`
- **Exemplo de Resposta (200 OK):**
```json
[
  {
    "veiculoId": 1,
    "modelo": "Corolla XEi 2.0",
    "anoFabricacao": 2023,
    "placa": "BRA2E19",
    "cor": "Prata",
    "quilometragem": 18500,
    "fabricante": "Toyota",
    "paisFabricante": "Japão",
    "categoria": "Sedan Médio",
    "valorDiariaBase": 175.00
  }
]
```

#### 4.7. Filtro 4: Relatório de Desempenho da Frota (`LEFT JOIN` + `INNER JOIN`)
- **Método HTTP:** `GET`
- **Rota:** `/api/veiculos/relatorio-frota`
- **Descrição:** Realiza `LEFT JOIN` entre `Veiculos` e `Alugueis`, combinado com `INNER JOIN` em `Fabricantes` e `Categorias`, computando o faturamento total e o total de contratos por carro, incluindo aqueles que nunca foram alugados.
- **Códigos de Resposta:** `200 OK`
- **Exemplo de Resposta (200 OK):**
```json
[
  {
    "veiculoId": 1,
    "modelo": "Corolla XEi 2.0",
    "placa": "BRA2E19",
    "fabricante": "Toyota",
    "categoria": "Sedan Médio",
    "statusAtual": "Disponivel",
    "totalLocacoes": 1,
    "totalFaturado": 700.00,
    "ultimoKmRegistrado": 18500
  },
  {
    "veiculoId": 2,
    "modelo": "Polo Track 1.0",
    "placa": "ABC1D23",
    "fabricante": "Volkswagen",
    "categoria": "Econômico",
    "statusAtual": "Disponivel",
    "totalLocacoes": 0,
    "totalFaturado": 0.00,
    "ultimoKmRegistrado": 8200
  }
]
```

---

### 5. Módulo de Clientes (`/api/clientes`)

#### 5.1. Listar Clientes
- **Método HTTP:** `GET`
- **Rota:** `/api/clientes`
- **Códigos de Resposta:** `200 OK`

#### 5.2. Obter Cliente por ID
- **Método HTTP:** `GET`
- **Rota:** `/api/clientes/{id}`
- **Códigos de Resposta:** `200 OK`, `404 Not Found`

#### 5.3. Cadastrar Cliente
- **Método HTTP:** `POST`
- **Rota:** `/api/clientes`
- **Corpo da Requisição (JSON):**
```json
{
  "nome": "Fernanda Ribeiro Alves",
  "cpf": "321.654.987-11",
  "email": "fernanda.alves@email.com",
  "telefone": "(31) 98888-7777",
  "cnh": "55566677788"
}
```
- **Códigos de Resposta:** `201 Created`, `400 Bad Request`, `409 Conflict`

#### 5.4. Atualizar Cliente
- **Método HTTP:** `PUT`
- **Rota:** `/api/clientes/{id}`
- **Códigos de Resposta:** `204 No Content`, `400 Bad Request`, `404 Not Found`, `409 Conflict`

#### 5.5. Excluir Cliente
- **Método HTTP:** `DELETE`
- **Rota:** `/api/clientes/{id}`
- **Códigos de Resposta:** `204 No Content`, `400 Bad Request`, `404 Not Found`

#### 5.6. Filtro 3: Relatório de Locações por Cliente (`LEFT JOIN`)
- **Método HTTP:** `GET`
- **Rota:** `/api/clientes/relatorio-locacoes`
- **Descrição:** Executa `LEFT JOIN` entre `Clientes` e `Alugueis`, retornando o perfil de consumo e quantidade de contratos de todos os clientes, inclusive clientes sem histórico de locação.
- **Códigos de Resposta:** `200 OK`
- **Exemplo de Resposta (200 OK):**
```json
[
  {
    "clienteId": 1,
    "nome": "Carlos Eduardo Silva",
    "cpf": "123.456.789-01",
    "email": "carlos.silva@email.com",
    "totalAlugueis": 1,
    "totalGasto": 700.00,
    "ultimaLocacao": "2025-03-01T08:00:00Z"
  },
  {
    "clienteId": 3,
    "nome": "Roberto Ferreira Lima",
    "cpf": "456.789.123-45",
    "email": "roberto.lima@email.com",
    "totalAlugueis": 0,
    "totalGasto": 0.00,
    "ultimaLocacao": null
  }
]
```

---

### 6. Módulo de Aluguéis (`/api/alugueis`)

#### 6.1. Listar Aluguéis
- **Método HTTP:** `GET`
- **Rota:** `/api/alugueis`
- **Códigos de Resposta:** `200 OK`

#### 6.2. Obter Aluguel por ID
- **Método HTTP:** `GET`
- **Rota:** `/api/alugueis/{id}`
- **Códigos de Resposta:** `200 OK`, `404 Not Found`

#### 6.3. Abrir Novo Aluguel (Retirada)
- **Método HTTP:** `POST`
- **Rota:** `/api/alugueis`
- **Descrição:** Cria o contrato de locação, calcula a previsão de dias e valor total, registra o odômetro inicial e altera o status do veículo para `Alugado`.
- **Corpo da Requisição (JSON):**
```json
{
  "clienteId": 1,
  "veiculoId": 2,
  "dataInicio": "2026-10-10T08:00:00Z",
  "dataPrevistaDevolucao": "2026-10-14T08:00:00Z",
  "valorDiaria": 110.00
}
```
- **Códigos de Resposta:** `201 Created`, `400 Bad Request`, `404 Not Found`, `409 Conflict`
- **Exemplo de Resposta (201 Created):**
```json
{
  "id": 3,
  "clienteId": 1,
  "clienteNome": "Carlos Eduardo Silva",
  "clienteCpf": "123.456.789-01",
  "veiculoId": 2,
  "veiculoModelo": "Polo Track 1.0",
  "veiculoPlaca": "ABC1D23",
  "dataInicio": "2026-10-10T08:00:00Z",
  "dataPrevistaDevolucao": "2026-10-14T08:00:00Z",
  "dataDevolucaoEfetiva": null,
  "kmInicial": 8200,
  "kmFinal": null,
  "valorDiaria": 110.00,
  "valorTotal": 440.00,
  "status": "Ativo"
}
```

#### 6.4. Registrar Devolução de Veículo
- **Método HTTP:** `PUT`
- **Rota:** `/api/alugueis/{id}/devolucao`
- **Descrição:** Registra o encerramento do contrato, valida se o odômetro final é coerente com o inicial, calcula o valor final com base nos dias reais de uso, atualiza o odômetro do veículo e altera seu status para `Disponivel`.
- **Parâmetros de Rota:** `id` (int, obrigatório)
- **Corpo da Requisição (JSON):**
```json
{
  "dataDevolucao": "2026-10-14T09:00:00Z",
  "kmFinal": 8650
}
```
- **Códigos de Resposta:** `200 OK`, `400 Bad Request`, `404 Not Found`
- **Exemplo de Resposta (200 OK):**
```json
{
  "mensagem": "Devolução registrada com sucesso.",
  "aluguelId": 3,
  "diasLocados": 4,
  "quilometragemPercorrida": 450,
  "valorFinal": 440.00
}
```

#### 6.5. Cancelar ou Excluir Aluguel
- **Método HTTP:** `DELETE`
- **Rota:** `/api/alugueis/{id}`
- **Descrição:** Cancela aluguel ativo (liberando o veículo) ou remove registro histórico.
- **Códigos de Resposta:** `200 OK`, `204 No Content`, `404 Not Found`

#### 6.6. Filtro 2: Histórico de Locações do Cliente (`INNER JOIN`)
- **Método HTTP:** `GET`
- **Rota:** `/api/alugueis/cliente/{clienteId}?status={status}`
- **Descrição:** Realiza `INNER JOIN` entre `Alugueis`, `Clientes`, `Veiculos` e `Fabricantes`, retornando todos os contratos com dados completos do automóvel.
- **Parâmetros de Rota:** `clienteId` (int, obrigatório)
- **Parâmetros de Consulta (Query):** `status` (StatusAluguel: `Ativo`, `Concluido`, `Cancelado` - opcional)
- **Códigos de Resposta:** `200 OK`, `404 Not Found`
- **Exemplo de Resposta (200 OK):**
```json
[
  {
    "aluguelId": 1,
    "clienteId": 1,
    "clienteNome": "Carlos Eduardo Silva",
    "cpf": "123.456.789-01",
    "modeloVeiculo": "Corolla XEi 2.0",
    "placaVeiculo": "BRA2E19",
    "fabricante": "Toyota",
    "dataInicio": "2025-03-01T08:00:00Z",
    "dataPrevistaDevolucao": "2025-03-05T08:00:00Z",
    "dataDevolucaoEfetiva": "2025-03-05T07:30:00Z",
    "kmInicial": 17900,
    "kmFinal": 18500,
    "valorDiaria": 175.00,
    "valorTotal": 700.00,
    "status": "Concluido"
  }
]
```

#### 6.7. Filtro 5: Situação Financeira dos Aluguéis (`LEFT JOIN` + `INNER JOIN`)
- **Método HTTP:** `GET`
- **Rota:** `/api/alugueis/status-pagamento?statusPagamento={status}`
- **Descrição:** Realiza `LEFT JOIN` entre `Alugueis` e `Pagamentos`, com `INNER JOIN` em `Clientes` e `Veiculos`, permitindo auditar quais contratos estão quitados, pendentes ou sem liquidação financeira.
- **Parâmetros de Consulta (Query):** `statusPagamento` (StatusPagamento: `Pendente`, `Pago`, `Cancelado` - opcional)
- **Códigos de Resposta:** `200 OK`
- **Exemplo de Resposta (200 OK):**
```json
[
  {
    "aluguelId": 1,
    "clienteNome": "Carlos Eduardo Silva",
    "veiculoModelo": "Corolla XEi 2.0",
    "placa": "BRA2E19",
    "dataInicio": "2025-03-01T08:00:00Z",
    "valorTotalAluguel": 700.00,
    "statusAluguel": "Concluido",
    "pagamentoId": 1,
    "valorPago": 700.00,
    "dataPagamento": "2025-03-05T08:00:00Z",
    "metodoPagamento": "Pix",
    "statusPagamento": "Pago",
    "situacaoFinanceira": "Quitado"
  },
  {
    "aluguelId": 2,
    "clienteNome": "Mariana Costa Santos",
    "veiculoModelo": "Tracker Premier 1.2 Turbo",
    "placa": "XYZ9K88",
    "dataInicio": "2025-03-10T09:00:00Z",
    "valorTotalAluguel": 1200.00,
    "statusAluguel": "Ativo",
    "pagamentoId": null,
    "valorPago": null,
    "dataPagamento": null,
    "metodoPagamento": null,
    "statusPagamento": null,
    "situacaoFinanceira": "Sem Registro de Pagamento"
  }
]
```
