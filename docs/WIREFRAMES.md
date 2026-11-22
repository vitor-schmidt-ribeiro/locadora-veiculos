# Especificação e Guia dos Wireframes do Sistema

**Projeto:** Sistema de Gerenciamento de Locadora de Veículos  
**Disciplina:** Programação Back-End / Desenvolvimento Web  
**Aluno:** Vitor Schmidt Ribeiro  
**Ferramenta Recomendada:** Figma (Vetores SVG nativos)

---

## 1. Concepção e Racional de Design

Os wireframes foram projetados em formato de média/alta fidelidade, adotando tons de cinza com foco na usabilidade, hierarquia de informações e cumprimento estrito de todos os requisitos funcionais solicitados:

1. **Arquitetura de Navegação:**
   - **Menu Lateral Fixo (Sidebar):** Proporciona acesso imediato aos módulos do sistema (Dashboard, Clientes, Veículos, Aluguéis, Fabricantes/Categorias e Consultas Avançadas).
   - **Barra Superior (Topbar):** Identificação do usuário logado e contexto da tela atual.
   - **Área Central Dinâmica:** Otimizada para leitura de dados tabulares e formulários de cadastro/edição.

2. **Requisitos de Filtro e Pesquisa:**
   - Em cumprimento direto às diretrizes do trabalho, todas as entidades contam com pesquisas avançadas parametrizadas em **pelo menos dois campos distintos**:
     - **Clientes:** Filtro por Nome e por CPF.
     - **Veículos:** Filtro por Modelo e por Placa.
     - **Aluguéis:** Filtro por Cliente e por Situação do Contrato (Ativo / Finalizado).
     - **Consultas com Joins:** Painel dedicado para parametrização dos 5 filtros avançados.

---

## 2. Relação dos Wireframes Produzidos

Os arquivos vetoriais encontram-se disponíveis na pasta `docs/wireframes/`:

1. **`01_dashboard.svg` (Painel de Controle e Métricas):**
   - Indicadores numéricos (KPIs): Total de Clientes, Frota Cadastrada, Veículos Alugados e Faturamento Ativo.
   - Tabela de locações recentes com status visual.
   - Indicador circular de taxa de ocupação da frota.

2. **`02_clientes.svg` (Gestão de Clientes):**
   - Barra de busca contendo duplo filtro (Nome e CPF).
   - Botão de ação rápida "+ Novo Cliente".
   - Tabela com ações inline de Edição e Exclusão com confirmação.

3. **`03_veiculos.svg` (Gestão de Frota):**
   - Barra de busca com filtros por Modelo, Placa e Categoria.
   - Badges coloridos indicando status operacional (Disponível, Alugado, Em Manutenção).
   - Botão para inclusão de novos veículos.

4. **`04_alugueis_devolucao.svg` (Contratos de Locação e Devoluções):**
   - Filtros por Cliente e Status da Locação.
   - Botão "+ Nova Locação" para abertura de contrato.
   - Ação direta de "Registrar Devolução", com entrada de odômetro final e cálculo das diárias.

5. **`05_consultas_avancadas.svg` (Painel de Consultas Avançadas / Relatórios):**
   - Interface em abas (Tabs) para execução e visualização dos 5 relatórios avançados construídos com `INNER JOIN` e `LEFT JOIN`.
   - Exibição tabular estruturada com cálculo de totais e indicadores financeiros.

---

## 3. Como Importar os Wireframes para o Figma (1 Minuto)

Para gerar o link de compartilhamento exigido na entrega:

1. Acesse o **[Figma](https://www.figma.com)** e crie um arquivo novo (*New design file*).
2. Abra a pasta `docs/wireframes/` no seu computador.
3. Selecione os 5 arquivos `.svg` e **arraste-os para dentro da tela do Figma**.
4. O Figma converterá automaticamente cada SVG em um *Frame* vetorial independente, 100% editável.
5. No canto superior direito do Figma, clique no botão **Share (Compartilhar)**.
6. Em *Who has access*, marque **"Anyone with the link can view"** (Qualquer pessoa com o link pode visualizar).
7. Clique em **Copy link (Copiar link)**. Esse link será adicionado no artigo SBC e na documentação final.
