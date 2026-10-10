# Roteiro de Apresentação do Projeto - Pitch Completo

**Projeto:** Sistema de Gerenciamento de Locadora de Veículos (API RESTful)  
**Aluno:** Vitor Schmidt Ribeiro  
**Tempo estimado de gravação:** 8 a 10 minutos (faixa exigida: 6 a 12 minutos)

---

## 1. Preparação da Tela Antes de Iniciar a Gravação

Deixe abertas as duas janelas que você já preparou:
1. **VS Code:** Com o repositório aberto, mostrando na barra lateral as pastas da solução (`Domain`, `Infrastructure`, `Api`, `docs`) e o arquivo `docs/MODELO_CONCEITUAL.md` aberto no editor.
2. **Navegador (Swagger UI):** Aberto em `http://localhost:5097/swagger/index.html` com os controladores carregados.

---

## 2. Roteiro Passo a Passo de Gravação e Fala

---

### Bloco 1: Introdução, Apresentação e Visão Geral (00:00 - 01:30)

* **O que mostrar na tela:**
  Comece com a tela do **VS Code** aberta (mostrando a estrutura de pastas do projeto à esquerda) e, em seguida, alterne rapidamente para o **Swagger UI** no navegador, mostrando que a aplicação já está em execução.

* **O que falar:**
  > "Olá, professor! Meu nome é Vitor Schmidt Ribeiro e hoje vou apresentar o projeto completo do Sistema de Gerenciamento de Locadora de Veículos.
  >
  > O objetivo principal deste projeto foi desenvolver uma solução de backend robusta para controlar todo o fluxo operacional de uma locadora de automóveis. A aplicação gerencia desde o controle de frota — organizando montadoras, categorias e veículos —, até a base de clientes, contratos de aluguel com cálculo de diárias e devolução, e o acompanhamento dos pagamentos.
  >
  > Como o senhor pode ver aqui no VS Code, a solução foi construída em C# com a plataforma .NET 8, estruturada em camadas bem definidas para garantir separação de responsabilidades. Para persistência de dados, utilizamos o Entity Framework Core 8 com banco de dados SQL Server Express, aplicando mapeamentos explícitos via Fluent API e migrações.
  >
  > E aqui no navegador, temos a documentação interativa e os testes dos endpoints totalmente integrados através do Swagger e OpenAPI."

---

### Bloco 2: Arquitetura em Camadas e Modelagem do Banco de Dados (01:30 - 03:30)

* **O que mostrar na tela:**
  Volte para o **VS Code** com o arquivo `docs/MODELO_CONCEITUAL.md` em foco, destacando a lista das 6 entidades e a árvore de pastas à esquerda (`src/LocadoraVeiculos.Domain`, `Infrastructure`, `Api`).

* **O que falar:**
  > "Passando para a arquitetura do projeto, organizamos o backend em três camadas principais:
  > - O projeto **Domain**, onde residem as entidades e enums de negócio puros, sem nenhuma dependência externa;
  > - O projeto **Infrastructure**, responsável pela persistência com o DbContext, configurações Fluent API, integridade relacional e migrations;
  > - E o projeto **Api**, onde ficam os controladores RESTful, DTOs de entrada e saída com Data Annotations e tratamento global de erros.
  >
  > Para o modelo de dados, estruturamos 6 entidades centrais:
  > 1. **Fabricante**: Cadastro das montadoras dos carros;
  > 2. **Categoria**: Classificação dos veículos (Econômico, Sedã, SUV), onde fica o valor base da diária;
  > 3. **Veículo**: O ativo da locadora, vinculado a fabricante e categoria, com placa única, quilometragem e controle de status (Disponível, Alugado ou Em Manutenção);
  > 4. **Cliente**: Registro do locatário, com validação de unicidade de CPF, e-mail e CNH;
  > 5. **Aluguel**: O contrato central que conecta o Cliente ao Veículo, guardando datas de retirada e devolução, quilometragens inicial e final, e o valor total;
  > 6. **Pagamento**: Registro financeiro do aluguel, contendo forma de pagamento e situação da transação."

---

### Bloco 3: Demonstração Prática no Swagger UI (03:30 - 08:00)

* **O que mostrar na tela:**
  Alterne para o navegador com o **Swagger UI** (`localhost:5097/swagger/index.html`).

* **O que falar:**
  > "Agora vamos para a demonstração prática da API em execução através do Swagger."

#### 1. Demonstração de CRUD (Clientes)
* **Ação no Swagger:**
  1. Abra a seção **Clientes** $ightarrow$ `GET /api/Clientes`.
  2. Clique em **Try it out** $ightarrow$ **Execute**. Mostre o retorno HTTP 200 com a lista de clientes.
  3. Abra `POST /api/Clientes`, clique em **Try it out** e execute com o JSON:
     ```json
     {
       "nome": "Carlos Eduardo",
       "cpf": "11122233344",
       "email": "carlos.eduardo@email.com",
       "telefone": "31988887777",
       "cnh": "99887766554"
     }
     ```
  4. Mostre a resposta HTTP 201 Created.
* **O que falar:**
  > "Aqui em Clientes temos o CRUD completo. Executando o GET, vemos a lista retornada com código 200. Ao cadastrar um novo cliente via POST, a API valida formato e duplicidade de CPF e CNH, retornando HTTP 201 Created com o recurso criado."

#### 2. Regra de Negócio: Locação e Devolução de Veículo
* **Ação no Swagger:**
  1. Vá até a seção **Alugueis** $ightarrow$ `POST /api/Alugueis`.
  2. Clique em **Try it out**, preencha e execute:
     ```json
     {
       "clienteId": 1,
       "veiculoId": 1,
       "dataPrevistaDevolucao": "2026-10-15T18:00:00"
     }
     ```
  3. Mostre o retorno HTTP 201 Created com o status `Ativo` e anote o `id` gerado (ex.: 3).
  4. Abra `PUT /api/Alugueis/{id}/devolucao`.
  5. Coloque o ID da locação e no corpo:
     ```json
     {
       "quilometragemFinal": 45350,
       "observacoes": "Veículo devolvido no prazo e com tanque cheio."
     }
     ```
  6. Clique em **Execute** e mostre a resposta HTTP 200 com o cálculo financeiro das diárias e a devolução concluída.
* **O que falar:**
  > "No fluxo de aluguel, temos regras de negócio críticas: ao abrir uma locação via POST, a API verifica se o veículo está disponível. Se estiver, cria o contrato com status Ativo e atualiza o veículo para Alugado, impedindo que seja alugado duas vezes.
  >
  > Na devolução, através do PUT, o sistema valida se a quilometragem final não é menor que a inicial, encerra o aluguel, calcula o valor total com base na diária da categoria e libera o veículo de volta para o status Disponível."

#### 3. Os 5 Endpoints com Filtros e Joins
* **Ação no Swagger:**
  Demonstre as 5 consultas avançadas solicitadas no projeto:

  * **Filtro 1:** `GET /api/Veiculos/disponiveis`
    * Preencha `categoriaId = 1`, `fabricanteId = 1` $ightarrow$ **Execute**.
    * **Fala:** *"O Filtro 1 executa um INNER JOIN entre Veículos, Categorias e Fabricantes, filtrando os carros disponíveis no pátio conforme o fabricante e categoria escolhidos."*

  * **Filtro 2:** `GET /api/Alugueis/cliente/{clienteId}`
    * Preencha `clienteId = 1` $ightarrow$ **Execute**.
    * **Fala:** *"O Filtro 2 realiza INNER JOIN entre Aluguéis, Clientes e Veículos, trazendo o extrato detalhado de locações de um cliente com o modelo e a placa do carro alugado."*

  * **Filtro 3:** `GET /api/Clientes/relatorio-locacoes`
    * Clique em **Execute**.
    * **Fala:** *"O Filtro 3 utiliza um LEFT JOIN entre Clientes e Aluguéis. Ele lista todos os clientes da base, inclusive aqueles que ainda não alugaram nenhum veículo, exibindo a contagem de contratos e o valor total gasto por cada um."*

  * **Filtro 4:** `GET /api/Veiculos/relatorio-frota`
    * Clique em **Execute**.
    * **Fala:** *"O Filtro 4 combina LEFT JOIN e INNER JOIN para exibir a taxa de utilização da frota, trazendo dados de fabricante, categoria e o total acumulado de locações de cada automóvel."*

  * **Filtro 5:** `GET /api/Alugueis/status-pagamento`
    * Preencha `statusPagamento = Pendente` ou `Aprovado` $ightarrow$ **Execute**.
    * **Fala:** *"E o Filtro 5 faz LEFT JOIN com a tabela de Pagamentos e INNER JOIN com Clientes e Veículos para auditar a situação financeira dos contratos, identificando aluguéis pagos ou com pendências."*

---

### Bloco 4: Conclusões e Encerramento (08:00 - 09:30)

* **O que mostrar na tela:**
  Volte para o **VS Code** ou mostre a tela do repositório no GitHub.

* **O que falar:**
  > "Para finalizar a apresentação, o projeto atendeu a todos os requisitos técnicos e de negócio:
  > - Modelagem relacional consistente no SQL Server Express com integridade referencial;
  > - Arquitetura em camadas desacopladas seguindo os padrões do .NET 8;
  > - APIs RESTful completas com DTOs, validações e controle de status HTTP;
  > - Consultas avançadas eficientes utilizando INNER JOIN e LEFT JOIN;
  > - E documentação viva com testes interativos via Swagger.
  >
  > Agradeço a atenção do senhor, professor, e as orientações ao longo do desenvolvimento deste trabalho. Muito obrigado!"
