# Roteiro de Gravação com Indicações de Tela (Teleprompter)

**Projeto:** Sistema de Gerenciamento de Locadora de Veículos (API RESTful em .NET 8 / C#)  
**Aluno:** Vitor Schmidt Ribeiro  
**Tempo estimado:** 8 a 9 minutos (limite da disciplina: 6 a 12 minutos)

---

## Como usar este roteiro:
- As instruções entre colchetes em destaque, como **`[Ação]`**, indicam o momento exato em que você deve alternar de tela, rolar o mouse ou clicar em um botão.
- O texto entre aspas é a sua fala contínua. Você pode ler diretamente ou usar como guia na gravação.

---

### Bloco 1: Abertura e Visão Geral (00:00 a 01:15)

**`[Tela no VS Code maximizado - mostrando a árvore de pastas à esquerda]`**

> "Olá, professor! Meu nome é Vitor Schmidt Ribeiro e hoje vou apresentar o projeto completo do Sistema de Gerenciamento de Locadora de Veículos.
>
> O objetivo central deste projeto foi conceber e construir uma solução robusta de backend voltada para gerenciar todo o ciclo operacional de uma locadora. A solução atende o controle detalhado de frota — organizando montadoras, categorias e veículos —, a gestão cadastral de clientes com checagem de documentos como CPF e CNH, a formalização de contratos de locação com cálculo de diárias e quilometragem, além do controle de devoluções e da situação financeira dos pagamentos.
>
> Como o senhor pode ver aqui no VS Code..."

**`[Apontar com o mouse para as pastas Domain, Infrastructure, Api]`**

> "...a aplicação foi desenvolvida em C# na plataforma .NET 8, com arquitetura em camadas e Entity Framework Core para persistência relacional."

**`[Alt + Tab: Ir para o Swagger no navegador]`**

> "E aqui no navegador, como o senhor pode acompanhar, a nossa API já se encontra em execução e documentada com Swagger."

---

### Bloco 2: Arquitetura e Modelagem do Banco (01:15 a 03:00)

**`[Alt + Tab: Voltar para o VS Code - abrir/destacar o arquivo docs/MODELO_CONCEITUAL.md]`**

> "Passando para a arquitetura do sistema, nós adotamos o padrão de divisão em camadas para garantir alta coesão e independência:
> - O projeto **LocadoraVeiculos.Domain**: contém as entidades puras e os enums de negócio, totalmente desacoplados de bibliotecas externas;
> - O projeto **LocadoraVeiculos.Infrastructure**: isola o acesso a dados via Entity Framework Core, mapeamentos explícitos com Fluent API, integridade relacional, chaves e migrações;
> - E o projeto **LocadoraVeiculos.Api**: expõe os controladores RESTful, DTOs de entrada e saída com validações declarativas e middlewares de tratamento de erros."

**`[Rolar um pouco o MODELO_CONCEITUAL.md para mostrar a lista das 6 entidades]`**

> "Para a modelagem relacional, estruturamos 6 entidades centrais:
> 1. **Fabricante**: Cadastro das montadoras dos veículos;
> 2. **Categoria**: Classificação dos carros (como Econômico, Sedã e SUV), onde fixamos o valor base da diária;
> 3. **Veículo**: O ativo físico da locadora, com placa única, ano, quilometragem e controle de status operacional (Disponível, Alugado ou Em Manutenção);
> 4. **Cliente**: Registro do locatário com regras estritas de unicidade em CPF, e-mail e CNH;
> 5. **Aluguel**: O contrato central que conecta o Cliente ao Veículo, guardando datas de retirada e devolução, quilometragens inicial e final, e o valor total calculado;
> 6. **Pagamento**: Registro financeiro do aluguel com método e situação da transação."

---

### Bloco 3: Demonstração Prática no Swagger (03:00 a 07:30)

**`[Alt + Tab: Ir para o Swagger no navegador]`**

> "Agora vamos ver a API em execução na prática através da interface do Swagger."

**`[Rolar a tela suavemente para mostrar as tags e ir até o rodapé nos Schemas]`**

> "Aqui no Swagger temos a documentação viva da API. Cada endpoint conta com sumário em português gerado via comentários XML no C#, mapeamento explícito de códigos HTTP com ProducesResponseType — cobrindo sucessos e erros como 400 Bad Request e 404 Not Found — e, no final da página, temos os Schemas documentando todos os tipos de dados dos nossos DTOs.
>
> Vamos agora aos testes práticos do ciclo de negócio."

---

#### 1. Teste de Clientes
**`[Subir a tela até a tag Clientes e clicar em GET /api/Clientes]`**  
**`[Clicar em Try it out e depois em Execute]`**

> "No controlador de Clientes, realizamos a listagem via GET recebendo 200 OK com os dados retornados."

**`[Fechar o GET e abrir POST /api/Clientes]`**  
**`[Clicar em Try it out e colar o JSON de teste:]`**
```json
{
  "nome": "Carlos Eduardo",
  "cpf": "11122233344",
  "email": "carlos.eduardo@email.com",
  "telefone": "31988887777",
  "cnh": "99887766554"
}
```
**`[Clicar em Execute e mostrar o retorno 201 Created]`**

> "Ao cadastrar um novo cliente via POST, a API executa validações de integridade no CPF e CNH, gravando o registro com código HTTP 201 Created."

---

#### 2. Teste da Frota de Veículos
**`[Ir na tag Veiculos e abrir GET /api/Veiculos]`**  
**`[Clicar em Try it out e depois em Execute]`**  
**`[Apontar com o mouse para o Veículo ID 1 - Gol 1.0, com status "Disponivel"]`**

> "No endpoint de Veículos, consultamos a frota ativa. Observe que o Veículo de ID 1, o Gol 1.0, está atualmente no pátio com status 'Disponivel' e 45 mil quilômetros rodados."

---

#### 3. Teste do Ciclo de Locação e Bloqueio de Regra (Erro 400)
**`[Ir na tag Alugueis e abrir POST /api/Alugueis]`**  
**`[Clicar em Try it out e preencher:]`**
- `clienteId`: 1
- `veiculoId`: 1
- `dataPrevistaDevolucao`: "2026-10-15T18:00:00"

**`[Clicar em Execute e mostrar o retorno 201 Created com status "Ativo"]`**

> "Agora vamos abrir um contrato para o Cliente 1 utilizando o Veículo 1. Ao executar o POST, a API cria o aluguel como Ativo e altera o veículo para 'Alugado', retornando 201 Created."

**`[Sem mudar nada, clicar em Execute novamente no mesmo botão]`**  
**`[Mostrar na tela o retorno HTTP 400 Bad Request em vermelho]`**

> "E para demonstrar a consistência das regras de negócio: se tentarmos alugar esse mesmo veículo novamente em seguida... ao clicar em Execute, a API bloqueia a operação retornando HTTP 400 Bad Request, informando que o veículo já se encontra alugado. Isso impede duplicidade de locação."

---

#### 4. Teste de Devolução do Veículo com Cálculo
**`[Abrir PUT /api/Alugueis/{id}/devolucao]`**  
**`[Clicar em Try it out, preencher id = 3 e no corpo:]`**
```json
{
  "quilometragemFinal": 45350,
  "observacoes": "Devolução realizada no prazo e com tanque cheio."
}
```
**`[Clicar em Execute e mostrar o retorno 200 OK com o valor total calculado]`**

> "Para encerrar o ciclo do aluguel, registramos a devolução via PUT. A API valida a quilometragem final para garantir que não seja inferior à inicial, calcula o valor financeiro multiplicando os dias pela diária da categoria do veículo, encerra o contrato e altera o status do carro de volta para 'Disponivel'."

---

#### 5. Execução dos 5 Filtros Avançados com Joins
**`[Abrir Filtro 1: GET /api/Veiculos/disponiveis]`**  
**`[Try it out -> categoriaId = 1, fabricanteId = 1 -> Execute]`**

> "O Filtro 1 executa INNER JOIN entre Veículos, Categorias e Fabricantes, filtrando apenas veículos disponíveis conforme os parâmetros selecionados."

**`[Abrir Filtro 2: GET /api/Alugueis/cliente/{clienteId}]`**  
**`[Try it out -> clienteId = 1 -> Execute]`**

> "O Filtro 2 realiza INNER JOIN entre Aluguéis, Clientes e Veículos, trazendo o extrato detalhado de locações do cliente com placa e modelo do automóvel."

**`[Abrir Filtro 3: GET /api/Clientes/relatorio-locacoes]`**  
**`[Try it out -> Execute direto]`**

> "O Filtro 3 aplica LEFT JOIN entre Clientes e Aluguéis. Ele é essencial pois lista toda a base de clientes, inclusive aqueles com zero locações, calculando a quantidade de contratos e o valor total acumulado."

**`[Abrir Filtro 4: GET /api/Veiculos/relatorio-frota]`**  
**`[Try it out -> Execute direto]`**

> "O Filtro 4 combina LEFT JOIN e INNER JOIN para exibir a taxa de utilização da frota, trazendo dados de fabricante, categoria e o total acumulado de locações de cada carro."

**`[Abrir Filtro 5: GET /api/Alugueis/status-pagamento]`**  
**`[Try it out -> statusPagamento = Pendente -> Execute]`**

> "E o Filtro 5 faz LEFT JOIN com Pagamentos e INNER JOIN com Clientes e Veículos para auditar os contratos quitados ou pendentes de pagamento."

---

### Bloco 4: Conclusões e Encerramento (07:30 a 08:30)

**`[Alt + Tab: Voltar para o VS Code - mostrando o projeto]`**

> "Como considerações finais, a aplicação atingiu plenamente todos os objetivos estabelecidos:
> - Modelagem relacional consistente e íntegra com SQL Server Express;
> - Arquitetura em camadas limpas, desacopladas e fortemente tipadas com .NET 8 e C#;
> - APIs RESTful completas com validações estritas em DTOs e códigos de status HTTP semânticos;
> - Consultas avançadas eficientes aplicando INNER JOIN e LEFT JOIN;
> - E documentação viva e testes interativos centralizados via Swagger.
>
> Agradeço a atenção do senhor, professor, e todas as orientações ao longo do desenvolvimento deste projeto. Muito obrigado!"
