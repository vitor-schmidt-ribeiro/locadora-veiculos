# Roteiro de Apresentação e Demonstração do Projeto (Pitch) - Etapa 4

**Instituição:** Pontifícia Universidade Católica de Minas Gerais (PUC Minas)  
**Curso:** Análise e Desenvolvimento de Sistemas  
**Disciplina:** Programação Back-End  
**Projeto:** Sistema de Locadora de Veículos (API RESTful em .NET 8 / C#)  
**Aluno:** Vitor Schmidt Ribeiro  
**Tempo estimado de gravação:** 8 a 10 minutos (faixa exigida: 6 a 12 minutos)

---

## 1. Preparação Prévia do Ambiente (Antes de Começar a Gravar)

Antes de iniciar a gravação da tela, realize as seguintes ações preparatórias:

1. **Janelas Abertas e Organizadas:**
   - **Janela 1 (Swagger UI):** Navegador aberto em `http://localhost:5000/swagger` com a documentação da API carregada e pronta.
   - **Janela 2 (IDE / Editor de Código):** VS Code ou Visual Studio com o projeto aberto, evidenciando a árvore de pastas em camadas (`Domain`, `Infrastructure`, `Api`, `docs`).
   - **Janela 3 (Documento de Arquitetura):** Arquivo `docs/MODELO_CONCEITUAL.md` aberto no editor (com a pré-visualização do diagrama ou tabela de entidades visível).

2. **Como Iniciar a Aplicação Backend:**
   No terminal, dentro da pasta do projeto, execute o comando:
   ```bash
   dotnet run --project src/LocadoraVeiculos.Api
   ```
   Certifique-se de que a mensagem indicando que a aplicação está ouvindo na porta local (`http://localhost:5000`) foi exibida.

3. **Configuração de Gravação:**
   - Resolução sugerida: 1080p (1920x1080).
   - Ferramenta sugerida: OBS Studio, gravação nativa do sistema operacional (Xbox Game Bar no Windows ou gravador de tela do Linux/Teams).
   - Teste seu microfone antes para garantir clareza no áudio.

---

## 2. Roteiro Passo a Passo de Execução e Fala (Minuto a Minuto)

---

### Bloco 1: Introdução e Contexto do Projeto (00:00 - 01:30)

* **O que mostrar na tela:**
  Abra a tela inicial com o `README.md` do repositório ou com a página inicial do Swagger no navegador.

* **O que falar (Fala sugerida):**
  > "Olá, professor e colegas. Meu nome é Vitor Schmidt Ribeiro, sou aluno do curso de Análise e Desenvolvimento de Sistemas da PUC Minas, e hoje vou apresentar a Etapa 4 do projeto prático da disciplina de Programação Back-End: o desenvolvimento da API RESTful para o Sistema de Gerenciamento de Locadora de Veículos.
  >
  > O objetivo central deste projeto foi conceber, modelar e implementar uma solução completa de backend voltada para a gestão operacional e financeira de uma locadora de automóveis. A solução atende desde o cadastramento e controle de frota (com fabricantes e categorias de veículos), o gerenciamento da base de clientes com checagem de documentos (CPF e CNH), até a formalização de contratos de aluguel e o controle de devoluções e pagamentos.
  >
  > O backend foi desenvolvido em C# utilizando a plataforma .NET 8, com o Entity Framework Core 8 para persistência relacional, banco de dados SQL Server Express e Swagger com OpenAPI para documentação e testes interativos."

---

### Bloco 2: Modelagem de Dados e Arquitetura do Backend (01:30 - 03:30)

* **O que mostrar na tela:**
  Alterne para o editor de código e abra o arquivo `docs/MODELO_CONCEITUAL.md`. Mostre o diagrama de entidades (DER) e em seguida passe rapidamente pela árvore de pastas da solução (`LocadoraVeiculos.Domain`, `LocadoraVeiculos.Infrastructure`, `LocadoraVeiculos.Api`).

* **O que falar (Fala sugerida):**
  > "Passando para a modelagem relacional, nós estruturamos o banco de dados com 6 entidades principais para garantir integridade e normalização:
  >
  > 1. **Fabricante**: Cadastro das montadoras dos veículos.
  > 2. **Categoria**: Segmentação dos veículos (como Econômico, SUV, Sedã Premium), onde definimos o valor base da diária.
  > 3. **Veículo**: O ativo físico da locadora, vinculado a um fabricante e uma categoria, contendo placa única, ano, quilometragem e o status operacional (Disponível, Alugado ou Em Manutenção).
  > 4. **Cliente**: Registro cadastral com validação estrita de dados únicos, como CPF, e-mail e CNH.
  > 5. **Aluguel**: O contrato de locação ligando Cliente e Veículo, contendo data de retirada, data prevista, data real de devolução, quilometragem inicial e final, e o valor total calculado.
  > 6. **Pagamento**: Registro financeiro do contrato, contendo a forma de pagamento (Cartão de Crédito, Débito, PIX ou Dinheiro) e a situação do pagamento (Pendente, Aprovado, Cancelado).
  >
  > Na arquitetura, adotamos o padrão em camadas desacopladas:
  > - O projeto **Domain** contém apenas as entidades puras e enums de negócio, sem dependências externas.
  > - O projeto **Infrastructure** isola o acesso a dados via Entity Framework Core, aplicando mapeamentos explícitos via Fluent API, controle de índices únicos, chaves estrangeiras com restrição referencial e migrações de banco.
  > - E o projeto **Api** expõe os controladores RESTful, DTOs de entrada e saída com Data Annotations para validação e middlewares de tratamento uniforme de exceções."

---

### Bloco 3: Demonstração Prática no Swagger UI (03:30 - 08:00)

* **O que mostrar na tela:**
  Mude para o navegador com o **Swagger UI** (`http://localhost:5000/swagger`).

* **O que falar (Fala sugerida):**
  > "Agora vamos ver a API em execução através da interface interativa do Swagger UI. Toda a documentação foi enriquecida com anotações XML, demonstrando os códigos de status HTTP apropriados para cada cenário de sucesso e erro."

#### 1. Demonstração de Operações CRUD (03:30 - 05:00)
* **Ação no Swagger:**
  1. Clique na tag **Clientes** e abra o endpoint `GET /api/Clientes`.
  2. Clique em **Try it out** e depois em **Execute**.
  3. Mostre o retorno HTTP 200 com a lista de clientes pré-cadastrados (seed).
  4. Abra o endpoint `POST /api/Clientes`, clique em **Try it out**, insira um payload de exemplo e clique em **Execute**:
     ```json
     {
       "nome": "Carlos Eduardo",
       "cpf": "11122233344",
       "email": "carlos.eduardo@email.com",
       "telefone": "31988887777",
       "cnh": "99887766554"
     }
     ```
  5. Mostre o retorno HTTP 201 Created com os dados do cliente recém-criado.
* **O que falar:**
  > "Aqui em Clientes temos o CRUD completo. Executando o GET recebemos a lista de clientes cadastrados. Ao efetuar um POST para cadastrar um novo cliente, a API valida formato e unicidade de CPF e CNH, retornando o código HTTP 201 Created."

#### 2. Regra de Negócio de Locação e Devolução (05:00 - 06:15)
* **Ação no Swagger:**
  1. Abra a tag **Alugueis** e vá até `POST /api/Alugueis`.
  2. Clique em **Try it out**, preencha os dados de abertura de locação e clique em **Execute**:
     ```json
     {
       "clienteId": 1,
       "veiculoId": 1,
       "dataPrevistaDevolucao": "2026-10-15T18:00:00Z"
     }
     ```
  3. Mostre a criação do aluguel com status `Ativo` e mencione que o veículo teve seu status automaticamente alterado para `Alugado`.
  4. Vá até `PUT /api/Alugueis/{id}/devolucao`.
  5. Preencha o `id` da locação e o payload com a quilometragem final e observações:
     ```json
     {
       "quilometragemFinal": 45300,
       "observacoes": "Veículo devolvido em perfeito estado e com tanque cheio."
     }
     ```
  6. Clique em **Execute** e mostre a resposta HTTP 200 com o cálculo automático do valor total com base no número de dias e diária da categoria, além da liberação do status do veículo de volta para `Disponível`.
* **O que falar:**
  > "No fluxo de aluguel, temos uma regra de negócio crítica: ao abrir uma locação via POST, o sistema verifica se o veículo está disponível. Se estiver, registra o contrato e atualiza o veículo para 'Alugado', impedindo duplicidade.
  >
  > Já no endpoint de devolução (`PUT /api/Alugueis/{id}/devolucao`), o sistema valida a quilometragem final para garantir que não seja inferior à inicial, encerra o contrato, calcula o valor financeiro correspondente às diárias e retorna o veículo ao status de 'Disponível' para novas locações."

#### 3. Demonstração dos 5 Filtros Específicos com Joins (06:15 - 08:00)
* **Ação no Swagger:**
  Mostre os 5 endpoints de consulta avançada implementados conforme os requisitos da disciplina:

  * **Filtro 1: Veículos Disponíveis por Categoria e Fabricante (`GET /api/Veiculos/disponiveis`)**
    - Execute passando `categoriaId = 1` e `fabricanteId = 1`.
    - **O que falar:**
      > "O Filtro 1 executa um `INNER JOIN` entre as tabelas Veículos, Categorias e Fabricantes, filtrando apenas veículos aptos para locação com base nos parâmetros solicitados."

  * **Filtro 2: Histórico de Locações do Cliente (`GET /api/Alugueis/cliente/{clienteId}`)**
    - Execute informando `clienteId = 1`.
    - **O que falar:**
      > "O Filtro 2 realiza `INNER JOIN` entre Aluguéis, Clientes e Veículos, retornando o extrato completo de contratos de um cliente com os dados do modelo e placa do veículo alugado."

  * **Filtro 3: Relatório de Clientes e Locações (`GET /api/Clientes/relatorio-locacoes`)**
    - Execute o endpoint.
    - **O que falar:**
      > "O Filtro 3 utiliza um `LEFT JOIN` entre Clientes e Aluguéis. Isso é fundamental no negócio, pois permite listar todos os clientes cadastrados na base, incluindo aqueles que ainda não realizaram nenhuma locação, calculando a quantidade total de contratos e o ticket médio de cada cliente."

  * **Filtro 4: Relatório de Frota e Desempenho (`GET /api/Veiculos/relatorio-frota`)**
    - Execute o endpoint.
    - **O que falar:**
      > "O Filtro 4 combina `LEFT JOIN` e `INNER JOIN` para gerar um panorama de utilização de cada veículo da frota, trazendo fabricante, categoria e o total acumulado de locações realizadas por cada carro."

  * **Filtro 5: Situação Financeira dos Aluguéis (`GET /api/Alugueis/status-pagamento`)**
    - Execute passando o parâmetro `statusPagamento = Pendente` ou `Aprovado`.
    - **O que falar:**
      > "E o Filtro 5 aplica `LEFT JOIN` com a tabela de Pagamentos e `INNER JOIN` com Clientes e Veículos para auditar o status financeiro de cada locação, identificando pagamentos quitados ou pendências financeiras."

---

### Bloco 4: Conclusões e Considerações Finais (08:00 - 09:30)

* **O que mostrar na tela:**
  Volte para o GitHub mostrando o repositório com as 4 etapas versionadas, ou mostre a tela de arquivos do projeto no editor.

* **O que falar (Fala sugerida):**
  > "Como considerações finais, o projeto atingiu integralmente os objetivos propostos para a disciplina de Programação Back-End.
  >
  > Entre os pontos fortes da implementação, destaco:
  > - O cumprimento das boas práticas de arquitetura em camadas e Clean Code;
  > - O isolamento dos contratos por meio de DTOs, evitando expor entidades de banco diretamente à Web;
  > - A consistência das transações e integridade de dados garantidas pelo Entity Framework Core com o SQL Server;
  > - A documentação completa e testes interativos centralizados via Swagger.
  >
  > Agradeço a atenção de todos e a orientação do corpo docente durante o semestre. Muito obrigado!"

---

## 3. Resumo dos Comandos para Executar Localmente

```bash
# 1. Navegar até a pasta do projeto
cd locadora-veiculos

# 2. Restaurar dependências e compilar a solução
dotnet restore
dotnet build

# 3. Aplicar migrações ao banco de dados SQL Server (se necessário)
dotnet ef database update --project src/LocadoraVeiculos.Infrastructure --startup-project src/LocadoraVeiculos.Api

# 4. Iniciar a API com o Swagger habilitado
dotnet run --project src/LocadoraVeiculos.Api
```

Acesso ao Swagger UI no navegador:
- `http://localhost:5000/swagger`
- ou `https://localhost:5001/swagger`
