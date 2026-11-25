import subprocess
import os

html_article = """<!DOCTYPE html>
<html lang="pt-BR">
<head>
<meta charset="utf-8">
<title>Desenvolvimento e Integração de Front-End Responsivo para Sistema de Gerenciamento de Locadora de Veículos</title>
<style>
  @page {
    size: A4 portrait;
    margin: 20mm 18mm 18mm 18mm;
    @bottom-center {
      content: counter(page);
      font-size: 10pt;
      font-family: "Times New Roman", Times, serif;
    }
  }

  * { box-sizing: border-box; }

  body {
    font-family: "Times New Roman", Times, serif;
    font-size: 10.5pt;
    line-height: 1.4;
    color: #000000;
    margin: 0;
    padding: 0;
  }

  .title-area {
    text-align: center;
    margin-bottom: 24px;
  }

  .title-area h1 {
    font-size: 16pt;
    font-weight: bold;
    margin: 0 0 12px 0;
    line-height: 1.3;
  }

  .author {
    font-size: 12pt;
    font-weight: bold;
    margin-bottom: 4px;
  }

  .address {
    font-size: 10pt;
    color: #222222;
    margin-bottom: 18px;
    line-height: 1.35;
  }

  .address a {
    color: #000000;
    text-decoration: none;
  }

  .abstract-box {
    margin: 0 18px 22px 18px;
    font-size: 9.5pt;
    text-align: justify;
    line-height: 1.35;
  }

  .abstract-box strong {
    font-weight: bold;
  }

  .abstract-box p {
    margin: 0 0 10px 0;
  }

  .content-body {
    text-align: justify;
  }

  h2 {
    font-size: 13pt;
    font-weight: bold;
    margin: 10px 0 4px 0;
    border-bottom: 1px solid #e0e0e0;
    padding-bottom: 3px;
    page-break-after: avoid;
  }

  h3 {
    font-size: 10.5pt;
    font-weight: bold;
    margin: 10px 0 4px 0;
    page-break-after: avoid;
  }

  p {
    margin: 0 0 10px 0;
    text-indent: 1.25cm;
  }

  p.no-indent {
    text-indent: 0;
  }

  ul, ol {
    margin: 6px 0 12px 25px;
    padding: 0;
  }

  li {
    margin-bottom: 4px;
    line-height: 1.35;
  }

  .figure-box {
    text-align: center;
    margin: 16px auto;
    page-break-inside: avoid;
  }

  .figure-box img {
    max-width: 65%;
    height: auto;
    border: 1px solid #cccccc;
    border-radius: 4px;
    box-shadow: 0 1px 3px rgba(0,0,0,0.08);
  }

  .caption {
    font-size: 9pt;
    font-style: italic;
    color: #333333;
    margin-top: 6px;
    text-align: center;
  }

  table.sbc-table {
    width: 100%;
    border-collapse: collapse;
    margin: 14px 0;
    font-size: 9pt;
    page-break-inside: avoid;
  }

  table.sbc-table th {
    border-top: 1.5px solid #000000;
    border-bottom: 1px solid #000000;
    padding: 6px 8px;
    text-align: left;
    font-weight: bold;
  }

  table.sbc-table td {
    padding: 5px 8px;
    border-bottom: 1px solid #e2e8f0;
    vertical-align: top;
  }

  table.sbc-table tr:last-child td {
    border-bottom: 1.5px solid #000000;
  }

  code {
    font-family: "Courier New", Courier, monospace;
    font-size: 9.5pt;
    background: #f1f5f9;
    padding: 1px 4px;
    border-radius: 3px;
  }

  .references {
    font-size: 9pt;
    line-height: 1.4;
  }

  .references p {
    text-indent: -1cm;
    margin-left: 1cm;
    margin-bottom: 8px;
  }
</style>
</head>
<body>

  <div class="title-area">
    <h1>Desenvolvimento e Integração de Front-End Responsivo para Sistema de Gerenciamento de Locadora de Veículos</h1>
    <div class="author">Vitor Schmidt Ribeiro</div>
    <div class="address">
      Pontifícia Universidade Católica de Minas Gerais (PUC Minas) &mdash; Campus Betim<br>
      Betim &mdash; MG &mdash; Brasil<br>
      <code>1026074@pucminas.edu.br</code>
    </div>
  </div>

  <div class="abstract-box">
    <p><strong>Resumo.</strong> <em>Este artigo apresenta a concepção, o projeto de interface e a implementação de uma aplicação front-end responsiva integrada a uma API RESTful para o gerenciamento de locadora de veículos, no contexto da disciplina de Programação Back-End. A aplicação cliente foi construída em arquitetura de página única (SPA) utilizando HTML5 semântico, CSS3, Bootstrap 5 e JavaScript Vanilla assíncrono (Fetch API), consumindo serviços desenvolvidos em C# (.NET 8 com Entity Framework Core) conectados a banco de dados relacional. O projeto abrangeu o ciclo completo de engenharia de software: elaboração prévia de wireframes no Figma em média fidelidade, implementação de operações completas de CRUD com buscas multifatoriais em múltiplos campos por entidade, validações cadastrais com máscaras em tempo real, além da visualização analítica dedicada para cinco consultas relacionais com junções (INNER JOIN e LEFT JOIN). Os resultados demonstram a segregação de responsabilidades entre cliente e servidor, integridade referencial estrita e aderência rigorosa às diretrizes de usabilidade projetadas.</em></p>
    <p><strong>Abstract.</strong> <em>This paper presents the conception, interface design, and implementation of a responsive front-end application integrated with a RESTful API for vehicle rental management, within the scope of the Back-End Programming course. The client application was constructed under a Single-Page Application (SPA) architecture utilizing semantic HTML5, CSS3, Bootstrap 5, and asynchronous Vanilla JavaScript (Fetch API), consuming services developed in C# (.NET 8 with Entity Framework Core) connected to a relational database. The project encompassed the complete software engineering lifecycle: preliminary wireframe design in Figma in medium fidelity, implementation of comprehensive CRUD operations with multi-field search filters for each entity, strict client-side validation with real-time input masks, and dedicated analytical dashboards for five relational queries with joins (INNER JOIN and LEFT JOIN). The results demonstrate effective separation of concerns between client and server, strict referential integrity, and adherence to user experience standards.</em></p>
  </div>

  <div class="content-body">
    <h2>1. Introdução</h2>
    <p>O gerenciamento moderno de frotas de automóveis exige sistemas computacionais que combinem alta confiabilidade na camada de persistência com interfaces visuais acessíveis e intuitivas para operadores e clientes. No segmento de locação de veículos, a ausência de sincronização em tempo real entre o pátio operacional e o setor de atendimento resulta em gargalos severos, tais como indisponibilidade inadvertida de veículos, erros no cálculo de diárias e inconsistências em contratos vigentes.</p>
    <p>Na primeira etapa deste trabalho (Trabalho Prático 1), foram estabelecidas as fundações do sistema por meio do desenvolvimento de uma API RESTful em C# utilizando o framework ASP.NET Core (.NET 8), apoiada pelo mapeamento objeto-relacional com Entity Framework Core. Foram mapeadas entidades de domínio cruciais como Fabricantes, Categorias, Veículos, Clientes, Aluguéis e Pagamentos, assegurando integridade referencial por chaves estrangeiras e índices de unicidade no banco de dados.</p>
    <p>O presente Trabalho Prático 2 tem como objetivo central conceber, projetar e implementar a camada de front-end totalmente integrada à referida API. O desenvolvimento pautou-se na construção prévia de wireframes no Figma para balizar a experiência do usuário (UX), seguida da codificação de uma aplicação web responsiva. A solução contempla operações de CRUD para todas as entidades, filtros de busca simultânea em no mínimo dois campos por entidade e um painel analítico para execução e exibição visual dos resultados de cinco consultas avançadas utilizando junções relacionais (<code>INNER JOIN</code> e <code>LEFT JOIN</code>).</p>
    <p>Este artigo está organizado da seguinte forma: a Seção 2 aborda a modelagem e a arquitetura de integração entre cliente e servidor; a Seção 3 detalha a concepção dos wireframes no Figma; a Seção 4 descreve a implementação do front-end e os mecanismos de validação; a Seção 5 expõe os resultados obtidos e a demonstração prática das consultas com junções; e a Seção 6 traz as considerações finais e os direcionamentos do projeto.</p>

    <h2>2. Modelagem do Sistema e Arquitetura de Integração</h2>
    <p>A arquitetura geral do sistema adota o padrão cliente-servidor com forte desacoplamento entre as camadas de apresentação e de negócios. A fundamentação do ecossistema baseia-se na comunicação via protocolo HTTP/1.1 sobre transporte TCP/IP, transmitindo cargas úteis exclusivamente em formato JSON (JavaScript Object Notation).</p>
    <p>O back-end opera como fornecedor de serviços stateless, expondo endpoints documentados interativamente via especificação OpenAPI (Swagger UI) na porta local 5097. As regras de negócio e de consistência foram encapsuladas em controladores dedicados: <code>ClientesController</code>, <code>VeiculosController</code>, <code>AlugueisController</code>, <code>FabricantesController</code> e <code>CategoriasController</code>.</p>
    <p>Para viabilizar a comunicação direta entre a interface executada no navegador e a API, foi implementada e configurada uma política permissiva de CORS (<em>Cross-Origin Resource Sharing</em>) no arquivo de inicialização <code>Program.cs</code>, liberando requisições com os verbos HTTP essenciais: <code>GET</code>, <code>POST</code>, <code>PUT</code> e <code>DELETE</code>. Além disso, implementou-se um middleware customizado de captura global de exceções (<code>ErrorHandlingMiddleware</code>), responsável por interceptar violações de chave primária e duplicidade de dados relacionais (ex.: CPF, e-mail ou placa já cadastrados) e converter erros técnicos do SQLite ou SQL Server em mensagens amigáveis em português com código de status HTTP 409 (<em>Conflict</em>).</p>
    <p>A Tabela 1 sintetiza as entidades de domínio modeladas, destacando os atributos e as restrições relacionais aplicadas no sistema.</p>

    <div class="caption">Tabela 1. Estrutura das entidades relacionais do sistema.</div>
    <table class="sbc-table">
      <thead>
        <tr>
          <th>Entidade</th>
          <th>Atributos Principais</th>
          <th>Restrições de Domínio</th>
        </tr>
      </thead>
      <tbody>
        <tr>
          <td><strong>Fabricante</strong></td>
          <td>Id, Nome, PaisOrigem</td>
          <td>Chave primária; Nome obrigatório</td>
        </tr>
        <tr>
          <td><strong>Categoria</strong></td>
          <td>Id, Nome, Descricao, ValorDiariaBase</td>
          <td>ValorDiariaBase &gt; 0; Nome único</td>
        </tr>
        <tr>
          <td><strong>Veiculo</strong></td>
          <td>Id, Modelo, Placa, Ano, Cor, Km, Status</td>
          <td>Placa única; FKs para Fabricante e Categoria</td>
        </tr>
        <tr>
          <td><strong>Cliente</strong></td>
          <td>Id, Nome, CPF, Email, Telefone, CNH</td>
          <td>CPF único (11 dígitos); Email e CNH únicos</td>
        </tr>
        <tr>
          <td><strong>Aluguel</strong></td>
          <td>Id, DataInicio, DataPrevista, DataDevolucao, KmInicial, KmFinal, ValorTotal</td>
          <td>Status (Ativo/Finalizado); FKs para Cliente e Veiculo</td>
        </tr>
        <tr>
          <td><strong>Pagamento</strong></td>
          <td>Id, ValorPago, DataPagamento, Metodo, Status</td>
          <td>FK para Aluguel; Método (Cartão/PIX/Dinheiro)</td>
        </tr>
      </tbody>
    </table>

    <h2>3. Concepção e Especificação dos Wireframes</h2>
    <p>Em estrita consonância com os requisitos metodológicos da disciplina, a interface gráfica foi precedida pela elaboração de wireframes em média fidelidade, concebidos em escala de cinzas e tons neutros para priorizar a usabilidade, a hierarquia da informação e a ergonomia de navegação antes da codificação visual.</p>
    <p>Os wireframes foram construídos de forma modular, organizando o fluxo de trabalho do operador da locadora em três áreas fundamentais: barra de navegação lateral persistente (Sidebar), área superior de filtros multifatoriais simultâneos e modais contextuais para fluxos de negócio como locação e devolução.</p>
    <p>A Figura 1 apresenta o wireframe do Dashboard Executivo concebido no Figma, detalhando os cartões de indicadores-chave de desempenho (KPIs) com total de clientes, frota total, contratos ativos e faturamento consolidado, além da relação de locações recentes.</p>

    <p>As Figuras 1 e 2 ilustram os wireframes concebidos no Figma para o Dashboard Executivo e para o módulo de Consultas Avançadas em abas tabulares, respectivamente.</p>

    <div style="display: flex; gap: 12px; margin: 12px 0; justify-content: space-between; page-break-inside: avoid;">
      <div style="flex: 1; text-align: center;">
        <img src="file:///home/vitor/locadora-veiculos/docs/artigo_sbc/figuras/01_dashboard.png" style="width: 100%; border: 1px solid #ccc; border-radius: 4px;">
        <div class="caption">Figura 1. Wireframe do Dashboard Executivo (Figma).</div>
      </div>
      <div style="flex: 1; text-align: center;">
        <img src="file:///home/vitor/locadora-veiculos/docs/artigo_sbc/figuras/06_consultas_avancadas.png" style="width: 100%; border: 1px solid #ccc; border-radius: 4px;">
        <div class="caption">Figura 2. Wireframe de Consultas Avançadas (Figma).</div>
      </div>
    </div>

    <p class="no-indent">O projeto completo de wireframes encontra-se compartilhado publicamente no Figma através do link oficial:<br>
    <a href="https://www.figma.com/design/4RvUybZX26RIYfpTQJoJzv/Trabalho-Pr%C3%A1tico-2?node-id=0-1&amp;t=VaKG7JrY3j0xc9lp-1">https://www.figma.com/design/4RvUybZX26RIYfpTQJoJzv/Trabalho-Pr%C3%A1tico-2</a></p>

    <h2>4. Implementação da Camada Front-End</h2>
    <p>A camada cliente foi desenvolvida sob o paradigma de Single-Page Application (SPA) pura, concentrada no diretório <code>frontend/</code>. Optou-se deliberadamente pelo não uso de frameworks JavaScript pesados (como Angular ou React), assegurando leveza, carregamento imediato e portabilidade nativa em qualquer navegador web moderno.</p>

    <h3>4.1. Pilha Tecnológica e Estrutura de Arquivos</h3>
    <p>A estrutura do front-end foi dividida em quatro componentes modulares:</p>
    <ul>
      <li><strong>Estruturação e Estilo:</strong> O arquivo <code>index.html</code> utiliza HTML5 semântico com seções alternáveis. O framework Bootstrap 5.3 gerencia o grid responsivo e os componentes visuais, enquanto <code>css/style.css</code> personaliza cartões, tipografia, badges e sombras.</li>
      <li><strong>Serviço de Comunicação REST (<code>js/api.js</code>):</strong> Classe estática <code>ApiService</code> encapsulando chamadas assíncronas via <code>fetch()</code>. Gerencia serialização JSON, envio de cabeçalhos padronizados e interceptação de erros HTTP.</li>
      <li><strong>Controlador de Aplicação (<code>js/app.js</code>):</strong> Mantém o estado global em memória (<code>AppState</code>), gerencia eventos de navegação, disparo de modais, escutas de filtros e operações de CRUD.</li>
      <li><strong>Módulo de Consultas Analíticas (<code>js/consultas.js</code>):</strong> Responsável por parametrizar e despachar as chamadas às cinco consultas avançadas com junções relacionais.</li>
    </ul>

    <h3>4.2. Validações em Tempo Real e Prevenção de Duplo Envio</h3>
    <p>Para garantir a integridade dos dados antes do tráfego de rede, foram incorporadas máscaras dinâmicas de digitação nos campos críticos:</p>
    <ul>
      <li><strong>CPF do Cliente:</strong> Máscara automática formatando o padrão <code>000.000.000-00</code> com limite rígido de 14 caracteres (11 dígitos numéricos), aplicada tanto no modal de cadastro quanto no campo de filtro da tabela.</li>
      <li><strong>Telefone:</strong> Formatação em tempo real no padrão <code>(00) 00000-0000</code> (15 caracteres para celulares) ou <code>(00) 0000-0000</code> (14 caracteres para fixos).</li>
      <li><strong>CNH:</strong> Validação numérica estrita com limite máximo de 11 caracteres numéricos.</li>
    </ul>
    <p>Adicionalmente, todos os formulários receberam mecanismo de proteção contra múltiplos envios (duplo clique): ao submeter um formulário, o botão de confirmação é desabilitado instantaneamente e recebe um indicador giratório (<em>spinner</em>) com o texto "Salvando...", sendo reabilitado somente após a conclusão da requisição na cláusula <code>finally</code>.</p>

    <h2>5. Resultados e Demonstração das Consultas com Joins</h2>
    <p>A aplicação foi integrada com êxito à API em execução local. A Figura 3 ilustra a tela de Dashboard do front-end em produção, exibindo dados computados diretamente do banco relacional, incluindo totalizador de frota, contagem de locações ativas e faturamento acumulado.</p>

    <p>As Figuras 3 e 4 apresentam as telas reais da aplicação integrada em operação: o Dashboard com indicadores sincronizados via API e a interface de Gestão de Clientes com filtro duplo e máscaras ativas.</p>

    <div style="display: flex; gap: 12px; margin: 12px 0; justify-content: space-between; page-break-inside: avoid;">
      <div style="flex: 1; text-align: center;">
        <img src="file:///home/vitor/locadora-veiculos/docs/artigo_sbc/figuras/frontend_dashboard.png" style="width: 100%; border: 1px solid #ccc; border-radius: 4px;">
        <div class="caption">Figura 3. Dashboard integrado à API RESTful.</div>
      </div>
      <div style="flex: 1; text-align: center;">
        <img src="file:///home/vitor/locadora-veiculos/docs/artigo_sbc/figuras/frontend_clientes.png" style="width: 100%; border: 1px solid #ccc; border-radius: 4px;">
        <div class="caption">Figura 4. Gestão de Clientes com filtro duplo.</div>
      </div>
    </div>

    <h3>5.1. Demonstração e Mapeamento das Consultas com Joins</h3>
    <p>Para cumprir a exigência mandatória de pelo menos cinco consultas utilizando junções relacionais (<code>INNER JOIN</code> e <code>LEFT JOIN</code>), desenvolveu-se o painel de Consultas Avançadas. A Tabela 2 detalha a correlação técnica de cada consulta implementada.</p>

    <div class="caption">Tabela 2. Mapeamento das 5 consultas relacionais avançadas integradas ao front-end.</div>
    <table class="sbc-table">
      <thead>
        <tr>
          <th>Consulta</th>
          <th>Rota da API (Endpoint)</th>
          <th>Tipo de Junção</th>
          <th>Objetivo no Negócio</th>
        </tr>
      </thead>
      <tbody>
        <tr>
          <td><strong>Filtro 1</strong></td>
          <td><code>/api/Veiculos/disponiveis</code></td>
          <td><em>INNER JOIN</em></td>
          <td>Veículos livres filtrados por Categoria e Montadora.</td>
        </tr>
        <tr>
          <td><strong>Filtro 2</strong></td>
          <td><code>/api/Alugueis/cliente/{id}</code></td>
          <td><em>INNER JOIN</em></td>
          <td>Histórico consolidado de locações de um cliente.</td>
        </tr>
        <tr>
          <td><strong>Filtro 3</strong></td>
          <td><code>/api/Clientes/relatorio-locacoes</code></td>
          <td><em>LEFT JOIN</em></td>
          <td>Relação de todos os clientes e faturamento gerado.</td>
        </tr>
        <tr>
          <td><strong>Filtro 4</strong></td>
          <td><code>/api/Veiculos/relatorio-frota</code></td>
          <td><em>LEFT + INNER</em></td>
          <td>Taxa de ocupação e quilometragem rodada da frota.</td>
        </tr>
        <tr>
          <td><strong>Filtro 5</strong></td>
          <td><code>/api/Alugueis/status-pagamento</code></td>
          <td><em>LEFT + INNER</em></td>
          <td>Auditoria financeira de contratos quitados e pendentes.</td>
        </tr>
      </tbody>
    </table>

    <p>A Figura 5 exibe a tela de Consultas Avançadas em operação, demonstrando a renderização limpa e profissional dos dados retornados pelas junções SQL, sem exposição desnecessária de termos técnicos ao usuário final.</p>

    <div class="figure-box">
      <img src="file:///home/vitor/locadora-veiculos/docs/artigo_sbc/figuras/frontend_consultas.png" alt="Front-end Consultas">
      <div class="caption">Figura 5. Painel de Consultas Avançadas exibindo resultados das junções relacionais.</div>
    </div>

    <h2>6. Considerações Finais</h2>
    <p>O Trabalho Prático 2 alcançou plenamente os objetivos pedagógicos e técnicos propostos pela disciplina de Programação Back-End. A construção do front-end a partir de wireframes no Figma proporcionou uma transição consistente entre a fase de design de interação e a codificação de software.</p>
    <p>A arquitetura em SPA aliada à Fetch API permitiu uma interface responsiva, com validações cadastrais instantâneas e tratamento elegante de erros de banco de dados. A integração ponta a ponta com a API em C# (.NET 8) comprovou a eficácia do desacoplamento arquitetural, mantendo a integridade referencial por meio de consultas otimizadas com <code>INNER JOIN</code> e <code>LEFT JOIN</code>.</p>

    <h2>Repositório e Wireframes</h2>
    <ul>
      <li><strong>Repositório no GitHub:</strong> <br><a href="https://github.com/vitor-schmidt-ribeiro/locadora-veiculos">https://github.com/vitor-schmidt-ribeiro/locadora-veiculos</a></li>
      <li><strong>Wireframes no Figma:</strong> <br><a href="https://www.figma.com/design/4RvUybZX26RIYfpTQJoJzv/Trabalho-Pr%C3%A1tico-2?node-id=0-1&amp;t=VaKG7JrY3j0xc9lp-1">https://www.figma.com/design/4RvUybZX26RIYfpTQJoJzv/Trabalho-Pr%C3%A1tico-2</a></li>
    </ul>

    <h2>Referências</h2>
    <div class="references">
      <p>Bootstrap Team (2024). <em>Bootstrap 5 Documentation: Responsive Front-End Framework</em>. Disponível em: <a href="https://getbootstrap.com/docs/5.3/">https://getbootstrap.com/docs/5.3/</a>.</p>
      <p>Elmasri, R. e Navathe, S. B. (2011). <em>Sistemas de Banco de Dados</em>. Pearson Addison Wesley, São Paulo, 6ª edição.</p>
      <p>Microsoft (2024). <em>Documentação Oficial do ASP.NET Core e Entity Framework Core</em>. Microsoft Learn. Disponível em: <a href="https://learn.microsoft.com/aspnet/core">https://learn.microsoft.com/aspnet/core</a>.</p>
      <p>Pressman, R. S. e Maxim, B. R. (2016). <em>Engenharia de Software: Uma Abordagem Profissional</em>. McGraw-Hill, Porto Alegre, 8ª edição.</p>
      <p>Sociedade Brasileira de Computação (SBC) (2024). <em>Padrões para Publicação de Artigos em Conferências da SBC</em>. Disponível em: <a href="https://www.sbc.org.br">https://www.sbc.org.br</a>.</p>
      <p>Sommerville, I. (2011). <em>Engenharia de Software</em>. Pearson, São Paulo, 9ª edição.</p>
    </div>
  </div>

</body>
</html>
"""

html_path = "/tmp/article_preview.html"
with open(html_path, "w", encoding="utf-8") as f:
    f.write(html_article)

out_pdf = "/home/vitor/Artigo_SBC_Trabalho_Pratico_2.pdf"
subprocess.run([
    "/usr/bin/google-chrome",
    "--headless",
    "--disable-gpu",
    "--no-sandbox",
    "--print-to-pdf-no-header",
    f"--print-to-pdf={out_pdf}",
    html_path
], check=True)

print(f"Artigo SBC em PDF gerado localmente em: {out_pdf}")