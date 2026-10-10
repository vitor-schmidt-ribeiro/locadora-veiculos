# Roteiro Master de Apresentação do Projeto (Pitch Completo)

**Projeto:** Sistema de Gerenciamento de Locadora de Veículos (API RESTful em .NET 8 / C#)  
**Aluno:** Vitor Schmidt Ribeiro  
**Tempo estimado total:** 8 a 9 minutos (faixa exigida pela disciplina: 6 a 12 minutos)

---

## 1. Organização das Janelas na Tela (Antes de Iniciar a Gravação)

Deixe as duas janelas prontas na sua área de trabalho:
1. **VS Code:** Repositório aberto com a barra lateral evidenciando a divisão em camadas (`Domain`, `Infrastructure`, `Api`, `docs`) e o arquivo `docs/MODELO_CONCEITUAL.md` aberto no editor com a lista de entidades e relacionamentos visível.
2. **Navegador (Google Chrome):** Aberto no Swagger UI em `http://localhost:5097/swagger/index.html` com a aplicação já em execução.

*Dica para a gravação:* Comece gravando o **VS Code**, use `Alt + Tab` para alternar fluidamente para o **Swagger** no momento dos testes práticos, e no final dê `Alt + Tab` de volta para o **VS Code** para encerrar.

---

## 2. Cronograma e Estrutura dos Blocos

* **Bloco 1 (00:00 - 01:15):** Abertura, Contexto do Negócio e Apresentação das Telas (1m15s)
* **Bloco 2 (01:15 - 03:00):** Arquitetura Backend em Camadas e Modelagem Relacional DER (1m45s)
* **Bloco 3 (03:00 - 07:30):** Demonstração Prática Completa no Swagger UI (4m30s)
  - 3.1: Visão Geral OpenAPI e Schemas
  - 3.2: Gestão de Clientes (GET e POST)
  - 3.3: Consulta da Frota de Veículos (GET)
  - 3.4: Abertura de Locação e Teste de Bloqueio de Regra (Erro 400 Bad Request)
  - 3.5: Devolução de Veículo e Cálculo de Diárias (PUT)
  - 3.6: Execução dos 5 Filtros Avançados com Joins (INNER JOIN e LEFT JOIN)
* **Bloco 4 (07:30 - 08:30):** Boas Práticas, Conclusão e Encerramento (1m00s)

---

## 3. Roteiro Fala a Fala e Ações na Tela

---

### Bloco 1: Abertura, Contexto do Negócio e Visão Geral (00:00 a 01:15)

* **O que mostrar na tela:**
  Inicie com o **VS Code** maximizado, mostrando na barra lateral as pastas do projeto (`Domain`, `Infrastructure`, `Api`, `docs`). Logo após citar a execução da aplicação, dê um `Alt + Tab` rápido para o **Swagger UI** no navegador mostrando a API ativa na porta 5097, e depois volte para o VS Code.

* **O que falar:**
  > "Olá, professor! Meu nome é Vitor Schmidt Ribeiro e hoje vou apresentar o projeto completo do Sistema de Gerenciamento de Locadora de Veículos.
  >
  > O objetivo central deste projeto foi conceber e construir uma solução robusta de backend voltada para gerenciar todo o ciclo operacional de uma locadora. A solução atende o controle detalhado de frota — organizando montadoras, categorias e veículos —, a gestão cadastral de clientes com checagem de documentos como CPF e CNH, a emissão de contratos de locação com cálculo de diárias e quilometragem, além do controle rigoroso de devoluções e situação dos pagamentos.
  >
  > Como o senhor pode ver aqui no VS Code, a aplicação foi desenvolvida em C# na plataforma .NET 8, com o Entity Framework Core 8 para persistência relacional no SQL Server Express e Swagger para documentação OpenAPI.
  >
  > E aqui no navegador, a nossa API já se encontra em execução e pronta para consumo."

---

### Bloco 2: Arquitetura Backend e Modelagem Relacional DER (01:15 a 03:00)

* **O que mostrar na tela:**
  No **VS Code**, mantenha em foco o arquivo `docs/MODELO_CONCEITUAL.md` (mostrando a lista das 6 entidades e a descrição relacional) e aponte para a estrutura de pastas em `src/`.

* **O que falar:**
  > "Passando para a arquitetura do sistema, adotamos o padrão de divisão em camadas desacopladas para manter alta coesão e baixo acoplamento:
  > - O projeto **LocadoraVeiculos.Domain**: contém as entidades puras e os enums de negócio, totalmente independentes de frameworks;
  > - O projeto **LocadoraVeiculos.Infrastructure**: gerencia o acesso a dados via Entity Framework Core, mapeamentos explícitos com Fluent API, integridade relacional, índices e migrações;
  > - E o projeto **LocadoraVeiculos.Api**: expõe os controladores RESTful, DTOs de entrada e saída com validação declarativa e middlewares de tratamento de exceções.
  >
  > Para a modelagem relacional, nós estruturamos o banco com 6 entidades principais:
  > 1. **Fabricante**: Cadastro das montadoras (como Volkswagen, Chevrolet, Fiat);
  > 2. **Categoria**: Classificação dos veículos (Econômico, Sedã, SUV), onde fica o valor base da diária;
  > 3. **Veículo**: O ativo físico da frota, com placa única, ano, quilometragem e controle de status operacional (Disponível, Alugado ou Em Manutenção);
  > 4. **Cliente**: Registro do locatário com regras de unicidade para CPF, e-mail e CNH;
  > 5. **Aluguel**: O contrato central associando Cliente e Veículo, contendo data de retirada, data prevista, devolução, quilometragem inicial e final, e o valor total;
  > 6. **Pagamento**: Registro financeiro do contrato com método e status da transação."

---

### Bloco 3: Demonstração Prática Completa no Swagger UI (03:00 a 07:30)

* **O que mostrar na tela:**
  Dê `Alt + Tab` e permaneça no navegador com o **Swagger UI** (`localhost:5097/swagger/index.html`).

---

#### 3.1. Visão Geral da Documentação e Schemas (03:00 - 03:30)
* **Ação na tela:**
  Role a página do Swagger mostrando as tags organizadas e depois desça rapidamente até o rodapé mostrando a seção **Schemas**.
* **O que falar:**
  > "Aqui no Swagger temos a documentação interativa da API. Cada endpoint conta com sumário em português vindo de comentários XML no C#, mapeamento de códigos de retorno com ProducesResponseType — incluindo sucessos e erros como 400 Bad Request e 404 Not Found — e, aqui no final da página, temos os Schemas documentando todos os tipos de dados dos DTOs.
  >
  > Vamos agora iniciar os testes práticos do ciclo de negócio."

---

#### 3.2. Gestão de Clientes (03:30 - 04:15)
* **Ação no Swagger:**
  1. Abra a tag **Clientes** $ightarrow$ `GET /api/Clientes` $ightarrow$ **Try it out** $ightarrow$ **Execute**.
     * Mostre o retorno HTTP 200 com os clientes já semeados.
  2. Abra `POST /api/Clientes` $ightarrow$ **Try it out** $ightarrow$ Cole o payload:
     ```json
     {
       "nome": "Carlos Eduardo",
       "cpf": "11122233344",
       "email": "carlos.eduardo@email.com",
       "telefone": "31988887777",
       "cnh": "99887766554"
     }
     ```
  3. Clique em **Execute** e mostre a resposta **HTTP 201 Created**.
* **O que falar:**
  > "No controlador de Clientes, realizamos a consulta via GET recebendo 200 OK. Ao cadastrar um novo cliente via POST, a API executa validações de formato e integridade no CPF e CNH, persistindo o registro no banco com código HTTP 201 Created."

---

#### 3.3. Consulta da Frota de Veículos (04:15 - 04:45)
* **Ação no Swagger:**
  1. Vá até a tag **Veiculos** $ightarrow$ `GET /api/Veiculos` $ightarrow$ **Try it out** $ightarrow$ **Execute**.
  2. Aponte no JSON retornado para o **Veículo ID 1** (Gol 1.0, Placa ABC1D23) destacando que o status atual é `Disponivel` e a quilometragem é 45000.
* **O que falar:**
  > "No endpoint de Veículos, consultamos a frota ativa da locadora. Repare que o Veículo de ID 1, o Gol 1.0, está atualmente com status 'Disponivel' e 45 mil quilômetros rodados."

---

#### 3.4. Ciclo de Locação e Teste de Bloqueio de Regra - Erro 400 (04:45 - 05:45)
* **Ação no Swagger:**
  1. Vá até a tag **Alugueis** $ightarrow$ `POST /api/Alugueis` $ightarrow$ **Try it out**.
  2. Preencha os campos para alugar o Veículo 1 para o Cliente 1:
     ```json
     {
       "clienteId": 1,
       "veiculoId": 1,
       "dataPrevistaDevolucao": "2026-10-15T18:00:00"
     }
     ```
  3. Clique em **Execute**. Mostre o retorno **HTTP 201 Created** com status `Ativo` e anote o ID do aluguel gerado (ex.: ID 3).
  4. **O Teste de Erro de Negócio:** Sem mudar nada no formulário, clique em **Execute novamente**.
  5. Mostre na tela a resposta imediata de **HTTP 400 Bad Request** com a mensagem informando que o veículo já se encontra alugado.
* **O que falar:**
  > "Agora vamos abrir um aluguel para o Cliente 1 utilizando o Veículo 1. Ao executar o POST, a API cria o contrato com status Ativo e atualiza o veículo para 'Alugado', retornando 201 Created.
  >
  > E para demonstrar a consistência das regras de negócio: se tentarmos abrir uma nova locação para esse mesmo veículo imediatamente... ao clicar em Execute, a API bloqueia a operação retornando HTTP 400 Bad Request, informando que o veículo já está alugado. Isso impede qualquer duplicidade no pátio."

---

#### 3.5. Devolução de Veículo e Cálculo de Diárias (05:45 - 06:30)
* **Ação no Swagger:**
  1. Abra `PUT /api/Alugueis/{id}/devolucao` $ightarrow$ **Try it out**.
  2. No campo `id`, coloque o ID do aluguel criado (ex.: `3`).
  3. No corpo da requisição, envie a quilometragem final e observação:
     ```json
     {
       "quilometragemFinal": 45350,
       "observacoes": "Devolução realizada no prazo e com tanque cheio."
     }
     ```
  4. Clique em **Execute** e mostre a resposta **HTTP 200 OK** com o valor total calculado e o status finalizado.
* **O que falar:**
  > "Para finalizar o ciclo do aluguel, vamos registrar a devolução via PUT. A API valida se a quilometragem final não é inferior à inicial, calcula o valor financeiro total multiplicando os dias pela diária da categoria, encerra o contrato e altera o status do veículo de volta para 'Disponivel' para novos clientes."

---

#### 3.6. Demonstração dos 5 Filtros Avançados com Joins (06:30 - 07:30)
* **Ação no Swagger:**
  Execute sequencialmente os 5 filtros implementados conforme a especificação da disciplina:

  * **Filtro 1:** `GET /api/Veiculos/disponiveis`
    * Preencha `categoriaId = 1`, `fabricanteId = 1` $ightarrow$ **Execute**.
    * **Fala:** *"O Filtro 1 executa INNER JOIN entre Veículos, Categorias e Fabricantes, filtrando apenas veículos disponíveis conforme os critérios escolhidos."*

  * **Filtro 2:** `GET /api/Alugueis/cliente/{clienteId}`
    * Preencha `clienteId = 1` $ightarrow$ **Execute**.
    * **Fala:** *"O Filtro 2 realiza INNER JOIN entre Aluguéis, Clientes e Veículos, trazendo o extrato de locações do cliente com dados completos do automóvel."*

  * **Filtro 3:** `GET /api/Clientes/relatorio-locacoes`
    * Clique em **Execute** diretamente.
    * **Fala:** *"O Filtro 3 aplica LEFT JOIN entre Clientes e Aluguéis. Isso permite listar toda a base de clientes, inclusive aqueles com zero locações, calculando o total de contratos e gastos."*

  * **Filtro 4:** `GET /api/Veiculos/relatorio-frota`
    * Clique em **Execute** diretamente.
    * **Fala:** *"O Filtro 4 combina LEFT JOIN e INNER JOIN para traçar o desempenho da frota, exibindo montadora, categoria e o total acumulado de locações de cada carro."*

  * **Filtro 5:** `GET /api/Alugueis/status-pagamento`
    * Preencha `statusPagamento = Pendente` ou `Aprovado` $ightarrow$ **Execute**.
    * **Fala:** *"E o Filtro 5 faz LEFT JOIN com Pagamentos e INNER JOIN com Clientes e Veículos para auditar os contratos quitados ou pendentes de pagamento."*

---

### Bloco 4: Conclusões Técnicas e Encerramento (07:30 a 08:30)

* **O que mostrar na tela:**
  Dê um `Alt + Tab` e volte para o **VS Code**, exibindo a raiz do projeto.

* **O que falar:**
  > "Como considerações finais, a aplicação atingiu plenamente todos os objetivos estabelecidos:
  > - Modelagem relacional consistente e íntegra com SQL Server Express;
  > - Arquitetura em camadas limpas, desacopladas e fortemente tipadas com .NET 8 e C#;
  > - APIs RESTful completas com validações estritas em DTOs e códigos de status HTTP semânticos;
  > - Consultas avançadas eficientes aplicando INNER JOIN e LEFT JOIN;
  > - E documentação viva e testes interativos centralizados via Swagger.
  >
  > Agradeço a atenção do senhor, professor, e todas as orientações ao longo do desenvolvimento deste projeto. Muito obrigado!"
