/**
 * Módulo de Consultas Avançadas (Relatórios com INNER JOIN e LEFT JOIN)
 * Sistema de Locadora de Veículos
 */

// 1. Filtro 1: Veículos Disponíveis por Categoria e Fabricante (INNER JOIN)
async function carregarFiltro1() {
  const catSelect = document.getElementById('f1-categoria').value;
  const fabSelect = document.getElementById('f1-fabricante').value;
  const tbody = document.getElementById('f1-tabela-corpo');
  const countBadge = document.getElementById('f1-total-badge');

  tbody.innerHTML = '<tr><td colspan="6" class="text-center py-4"><div class="spinner-border text-primary spinner-border-sm me-2"></div>Carregando veículos disponíveis...</td></tr>';

  try {
    const dados = await ApiService.getVeiculosDisponiveis(catSelect, fabSelect);
    countBadge.textContent = `${dados.length} veículo(s)`;

    if (dados.length === 0) {
      tbody.innerHTML = '<tr><td colspan="6" class="text-center text-muted py-4">Nenhum veículo disponível encontrado para os filtros selecionados.</td></tr>';
      return;
    }

    tbody.innerHTML = dados.map(v => `
      <tr>
        <td class="fw-bold">${v.modelo}</td>
        <td><span class="badge bg-secondary font-monospace">${v.placa}</span></td>
        <td>${v.fabricante} (${v.paisFabricante || 'N/A'})</td>
        <td><span class="badge bg-info text-dark">${v.categoria}</span></td>
        <td>R$ ${Number(v.valorDiariaBase).toFixed(2)}</td>
        <td>${Number(v.quilometragem).toLocaleString('pt-BR')} km</td>
      </tr>
    `).join('');
  } catch (err) {
    tbody.innerHTML = `<tr><td colspan="6" class="text-center text-danger py-4">Erro ao carregar dados: ${err.message}</td></tr>`;
  }
}

// 2. Filtro 2: Histórico de Locações do Cliente (INNER JOIN)
async function carregarFiltro2() {
  const clienteId = document.getElementById('f2-cliente').value;
  const status = document.getElementById('f2-status').value;
  const tbody = document.getElementById('f2-tabela-corpo');
  const countBadge = document.getElementById('f2-total-badge');

  if (!clienteId) {
    tbody.innerHTML = '<tr><td colspan="7" class="text-center text-muted py-4">Selecione um cliente para visualizar o histórico.</td></tr>';
    countBadge.textContent = '0 registro(s)';
    return;
  }

  tbody.innerHTML = '<tr><td colspan="7" class="text-center py-4"><div class="spinner-border text-primary spinner-border-sm me-2"></div>Consultando histórico...</td></tr>';

  try {
    const dados = await ApiService.getHistoricoCliente(clienteId, status);
    countBadge.textContent = `${dados.length} contrato(s)`;

    if (dados.length === 0) {
      tbody.innerHTML = '<tr><td colspan="7" class="text-center text-muted py-4">Nenhum contrato encontrado para este cliente com os filtros aplicados.</td></tr>';
      return;
    }

    tbody.innerHTML = dados.map(item => `
      <tr>
        <td class="fw-bold">#${item.aluguelId}</td>
        <td>${item.clienteNome}</td>
        <td>${item.veiculoModelo} (${item.fabricante})</td>
        <td><span class="badge bg-secondary font-monospace">${item.veiculoPlaca}</span></td>
        <td>${new Date(item.dataRetirada).toLocaleDateString('pt-BR')}</td>
        <td>${item.dataDevolucaoEfetiva ? new Date(item.dataDevolucaoEfetiva).toLocaleDateString('pt-BR') : 'Em andamento'}</td>
        <td>
          <span class="badge ${item.statusAluguel === 'Ativo' ? 'bg-warning text-dark' : 'bg-success'}">
            ${item.statusAluguel}
          </span>
        </td>
      </tr>
    `).join('');
  } catch (err) {
    tbody.innerHTML = `<tr><td colspan="7" class="text-center text-danger py-4">Erro ao carregar histórico: ${err.message}</td></tr>`;
  }
}

// 3. Filtro 3: Relatório de Clientes e Locações (LEFT JOIN)
async function carregarFiltro3() {
  const tbody = document.getElementById('f3-tabela-corpo');
  const countBadge = document.getElementById('f3-total-badge');
  const filtroNome = document.getElementById('f3-filtro-nome').value.toLowerCase();

  tbody.innerHTML = '<tr><td colspan="6" class="text-center py-4"><div class="spinner-border text-primary spinner-border-sm me-2"></div>Gerando relatório de clientes (LEFT JOIN)...</td></tr>';

  try {
    const dados = await ApiService.getRelatorioLocacoesClientes();
    const filtrados = dados.filter(c => c.nome.toLowerCase().includes(filtroNome));
    countBadge.textContent = `${filtrados.length} cliente(s)`;

    if (filtrados.length === 0) {
      tbody.innerHTML = '<tr><td colspan="6" class="text-center text-muted py-4">Nenhum cliente encontrado.</td></tr>';
      return;
    }

    tbody.innerHTML = filtrados.map(c => `
      <tr>
        <td class="fw-bold">#${c.clienteId}</td>
        <td>${c.nome}</td>
        <td>${c.cpf}</td>
        <td>
          <span class="badge ${c.totalAlugueis > 0 ? 'bg-primary' : 'bg-secondary'}">
            ${c.totalAlugueis} locação(ões)
          </span>
        </td>
        <td class="fw-bold text-success">R$ ${Number(c.totalGasto || 0).toFixed(2)}</td>
        <td>${c.ultimaLocacao ? new Date(c.ultimaLocacao).toLocaleDateString('pt-BR') : '<span class="text-muted">Nunca alugou</span>'}</td>
      </tr>
    `).join('');
  } catch (err) {
    tbody.innerHTML = `<tr><td colspan="6" class="text-center text-danger py-4">Erro ao gerar relatório: ${err.message}</td></tr>`;
  }
}

// 4. Filtro 4: Relatório de Frota e Desempenho (LEFT JOIN + INNER JOIN)
async function carregarFiltro4() {
  const tbody = document.getElementById('f4-tabela-corpo');
  const countBadge = document.getElementById('f4-total-badge');

  tbody.innerHTML = '<tr><td colspan="6" class="text-center py-4"><div class="spinner-border text-primary spinner-border-sm me-2"></div>Carregando desempenho da frota...</td></tr>';

  try {
    const dados = await ApiService.getRelatorioFrota();
    countBadge.textContent = `${dados.length} veículo(s)`;

    if (dados.length === 0) {
      tbody.innerHTML = '<tr><td colspan="6" class="text-center text-muted py-4">Nenhum veículo encontrado na frota.</td></tr>';
      return;
    }

    tbody.innerHTML = dados.map(v => `
      <tr>
        <td class="fw-bold">${v.modelo}</td>
        <td><span class="badge bg-secondary font-monospace">${v.placa}</span></td>
        <td>${v.fabricante}</td>
        <td><span class="badge bg-info text-dark">${v.categoria}</span></td>
        <td>
          <span class="badge ${v.totalLocacoes > 0 ? 'bg-primary' : 'bg-light text-dark border'}">
            ${v.totalLocacoes} locação(ões)
          </span>
        </td>
        <td>
          <span class="badge ${v.statusAtual === 'Disponivel' ? 'bg-success' : (v.statusAtual === 'Alugado' ? 'bg-warning text-dark' : 'bg-danger')}">
            ${v.statusAtual}
          </span>
        </td>
      </tr>
    `).join('');
  } catch (err) {
    tbody.innerHTML = `<tr><td colspan="6" class="text-center text-danger py-4">Erro ao carregar relatório: ${err.message}</td></tr>`;
  }
}

// 5. Filtro 5: Situação Financeira dos Aluguéis (LEFT JOIN + INNER JOIN)
async function carregarFiltro5() {
  const statusPag = document.getElementById('f5-status-pagamento').value;
  const tbody = document.getElementById('f5-tabela-corpo');
  const countBadge = document.getElementById('f5-total-badge');

  tbody.innerHTML = '<tr><td colspan="7" class="text-center py-4"><div class="spinner-border text-primary spinner-border-sm me-2"></div>Consultando situação financeira...</td></tr>';

  try {
    const dados = await ApiService.getAlugueisPorStatusPagamento(statusPag);
    countBadge.textContent = `${dados.length} registro(s)`;

    if (dados.length === 0) {
      tbody.innerHTML = '<tr><td colspan="7" class="text-center text-muted py-4">Nenhum contrato encontrado para o status de pagamento selecionado.</td></tr>';
      return;
    }

    tbody.innerHTML = dados.map(item => `
      <tr>
        <td class="fw-bold">#${item.aluguelId}</td>
        <td>${item.clienteNome}</td>
        <td>${item.veiculoModelo} (${item.veiculoPlaca})</td>
        <td class="fw-bold">R$ ${Number(item.valorTotalAluguel || 0).toFixed(2)}</td>
        <td>${item.formaPagamento || '<span class="text-muted">Aguardando</span>'}</td>
        <td>
          <span class="badge ${item.statusPagamento === 'Aprovado' ? 'bg-success' : (item.statusPagamento === 'Pendente' ? 'bg-warning text-dark' : 'bg-secondary')}">
            ${item.statusPagamento || 'Não registrado'}
          </span>
        </td>
        <td>${item.dataPagamento ? new Date(item.dataPagamento).toLocaleDateString('pt-BR') : '-'}</td>
      </tr>
    `).join('');
  } catch (err) {
    tbody.innerHTML = `<tr><td colspan="7" class="text-center text-danger py-4">Erro ao carregar situação financeira: ${err.message}</td></tr>`;
  }
}
