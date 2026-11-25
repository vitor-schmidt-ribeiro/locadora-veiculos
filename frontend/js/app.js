/**
 * Aplicação Front-end - Sistema de Gerenciamento de Locadora de Veículos
 * Controle de Telas, Estados e Ações de CRUD
 */

// Estado Global da Aplicação
const AppState = {
  currentView: 'dashboard',
  clientes: [],
  veiculos: [],
  fabricantes: [],
  categorias: [],
  alugueis: []
};

// Inicialização ao carregar a página
document.addEventListener('DOMContentLoaded', async () => {
  setupNavigation();
  setupFilterListeners();
  setupInputMasks();
  await loadInitialData();
  const hash = window.location.hash.replace('#', '');
  if (hash && ['dashboard', 'clientes', 'veiculos', 'alugueis', 'fabricantes-categorias', 'consultas'].includes(hash)) {
    showView(hash);
  } else {
    showView('dashboard');
  }
});

// Funções de Máscara e Formatação
function formatarCPF(v) {
  if (!v) return '';
  v = v.toString().replace(/\D/g, '').slice(0, 11);
  if (v.length > 9) {
    return v.replace(/(\d{3})(\d{3})(\d{3})(\d{1,2})/, '$1.$2.$3-$4');
  }
  if (v.length > 6) {
    return v.replace(/(\d{3})(\d{3})(\d{1,3})/, '$1.$2.$3');
  }
  if (v.length > 3) {
    return v.replace(/(\d{3})(\d{1,3})/, '$1.$2');
  }
  return v;
}

function formatarTelefone(v) {
  if (!v) return '';
  v = v.toString().replace(/\D/g, '').slice(0, 11);
  if (v.length > 10) {
    return v.replace(/(\d{2})(\d{5})(\d{4})/, '($1) $2-$3');
  }
  if (v.length > 6) {
    return v.replace(/(\d{2})(\d{4})(\d{0,4})/, '($1) $2-$3');
  }
  if (v.length > 2) {
    return v.replace(/(\d{2})(\d{0,5})/, '($1) $2');
  }
  if (v.length > 0) {
    return v.replace(/(\d{0,2})/, '($1');
  }
  return v;
}

function formatarCNH(v) {
  if (!v) return '';
  return v.toString().replace(/\D/g, '').slice(0, 11);
}

// Toast / Notificações
function showToast(message, type = 'success') {
  const container = document.getElementById('toast-container');
  const bgClass = type === 'success' ? 'bg-success text-white' : (type === 'danger' ? 'bg-danger text-white' : 'bg-warning text-dark');
  
  const toastId = 'toast-' + Date.now();
  const toastHtml = `
    <div id="${toastId}" class="toast align-items-center ${bgClass} border-0 shadow" role="alert" aria-live="assertive" aria-atomic="true">
      <div class="d-flex">
        <div class="toast-body font-weight-medium">
          ${message}
        </div>
        <button type="button" class="btn-close btn-close-white me-2 m-auto" data-bs-dismiss="toast" aria-label="Close"></button>
      </div>
    </div>
  `;
  container.insertAdjacentHTML('beforeend', toastHtml);
  const toastEl = document.getElementById(toastId);
  const bsToast = new bootstrap.Toast(toastEl, { delay: 4000 });
  bsToast.show();
  toastEl.addEventListener('hidden.bs.toast', () => toastEl.remove());
}

// Configuração da Navegação da Sidebar
function setupNavigation() {
  const links = document.querySelectorAll('#sidebar .nav-link');
  links.forEach(link => {
    link.addEventListener('click', (e) => {
      e.preventDefault();
      const targetView = link.getAttribute('data-view');
      showView(targetView);
    });
  });
}

function showView(viewName) {
  AppState.currentView = viewName;

  // Atualizar visual da sidebar
  document.querySelectorAll('#sidebar .nav-link').forEach(link => {
    if (link.getAttribute('data-view') === viewName) {
      link.classList.add('active');
    } else {
      link.classList.remove('active');
    }
  });

  // Ocultar todas as views e exibir a selecionada
  document.querySelectorAll('.view-section').forEach(view => {
    view.classList.add('d-none');
  });

  const activeSection = document.getElementById(`view-${viewName}`);
  if (activeSection) {
    activeSection.classList.remove('d-none');
  }

  // Recarregar dados específicos da view
  switch (viewName) {
    case 'dashboard':
      renderDashboard();
      break;
    case 'clientes':
      renderClientes();
      break;
    case 'veiculos':
      renderVeiculos();
      break;
    case 'alugueis':
      renderAlugueis();
      break;
    case 'fabricantes-categorias':
      renderFabricantesCategorias();
      break;
    case 'consultas':
      initConsultasView();
      break;
  }
}

// Carregamento de Dados da API
async function loadInitialData() {
  try {
    const [clientes, veiculos, fabricantes, categorias, alugueis] = await Promise.all([
      ApiService.getClientes().catch(() => []),
      ApiService.getVeiculos().catch(() => []),
      ApiService.getFabricantes().catch(() => []),
      ApiService.getCategorias().catch(() => []),
      ApiService.getAlugueis().catch(() => [])
    ]);

    AppState.clientes = clientes;
    AppState.veiculos = veiculos;
    AppState.fabricantes = fabricantes;
    AppState.categorias = categorias;
    AppState.alugueis = alugueis;
  } catch (error) {
    showToast('Erro ao carregar dados iniciais da API: ' + error.message, 'danger');
  }
}

// --- 1. DASHBOARD ---
function renderDashboard() {
  document.getElementById('dash-total-clientes').textContent = AppState.clientes.length;
  document.getElementById('dash-total-veiculos').textContent = AppState.veiculos.length;

  const alugados = AppState.veiculos.filter(v => v.status === 'Alugado').length;
  const disponiveis = AppState.veiculos.filter(v => v.status === 'Disponivel').length;
  document.getElementById('dash-veiculos-alugados').textContent = alugados;
  document.getElementById('dash-veiculos-disponiveis').textContent = disponiveis;

  const totalFaturamento = AppState.alugueis
    .filter(a => a.valorTotal)
    .reduce((acc, a) => acc + (Number(a.valorTotal) || 0), 0);
  document.getElementById('dash-faturamento').textContent = `R$ ${totalFaturamento.toFixed(2)}`;

  // Tabela de Aluguéis Recentes
  const tbody = document.getElementById('dash-recentes-corpo');
  if (AppState.alugueis.length === 0) {
    tbody.innerHTML = '<tr><td colspan="5" class="text-center text-muted py-3">Nenhum aluguel registrado até o momento.</td></tr>';
    return;
  }

  tbody.innerHTML = AppState.alugueis.slice(0, 5).map(a => `
    <tr>
      <td class="fw-bold">#${a.id}</td>
      <td>${a.clienteNome || 'Cliente #' + a.clienteId}</td>
      <td>${a.veiculoModelo || 'Veículo #' + a.veiculoId} <span class="badge bg-secondary font-monospace">${a.veiculoPlaca || ''}</span></td>
      <td>${new Date(a.dataRetirada).toLocaleDateString('pt-BR')}</td>
      <td>
        <span class="badge ${a.status === 'Ativo' ? 'bg-warning text-dark' : 'bg-success'}">
          ${a.status}
        </span>
      </td>
    </tr>
  `).join('');
}

// --- 2. CLIENTES (CRUD + 2 Filtros: Nome e CPF) ---
function renderClientes() {
  const filtroNome = document.getElementById('filtro-cliente-nome').value.toLowerCase();
  const filtroCpf = document.getElementById('filtro-cliente-cpf').value.trim();

  const filtrados = AppState.clientes.filter(c => {
    const matchNome = !filtroNome || (c.nome && c.nome.toLowerCase().includes(filtroNome));
    const matchCpf = !filtroCpf || (c.cpf && c.cpf.includes(filtroCpf));
    return matchNome && matchCpf;
  });

  const tbody = document.getElementById('tabela-clientes-corpo');
  document.getElementById('badge-total-clientes').textContent = `${filtrados.length} cliente(s)`;

  if (filtrados.length === 0) {
    tbody.innerHTML = '<tr><td colspan="7" class="text-center text-muted py-4">Nenhum cliente encontrado com os filtros aplicados.</td></tr>';
    return;
  }

  tbody.innerHTML = filtrados.map(c => `
    <tr>
      <td class="fw-bold">#${c.id}</td>
      <td>${c.nome}</td>
      <td><span class="font-monospace">${formatarCPF(c.cpf)}</span></td>
      <td><span class="font-monospace">${c.cnh || '-'}</span></td>
      <td>${c.email || '-'}</td>
      <td>${formatarTelefone(c.telefone)}</td>
      <td class="text-end">
        <button class="btn btn-sm btn-outline-primary me-1" onclick="abrirModalEdicaoCliente(${c.id})">Editar</button>
        <button class="btn btn-sm btn-outline-danger" onclick="excluirCliente(${c.id})">Excluir</button>
      </td>
    </tr>
  `).join('');
}

function abrirModalNovoCliente() {
  document.getElementById('cliente-modal-title').textContent = 'Cadastrar Novo Cliente';
  document.getElementById('form-cliente').reset();
  document.getElementById('cliente-id').value = '';
  const modal = new bootstrap.Modal(document.getElementById('modal-cliente'));
  modal.show();
}

function abrirModalEdicaoCliente(id) {
  const cliente = AppState.clientes.find(c => c.id === id);
  if (!cliente) return;

  document.getElementById('cliente-modal-title').textContent = 'Editar Dados do Cliente';
  document.getElementById('cliente-id').value = cliente.id;
  document.getElementById('cliente-nome').value = cliente.nome || '';
  document.getElementById('cliente-cpf').value = formatarCPF(cliente.cpf || '');
  document.getElementById('cliente-cnh').value = formatarCNH(cliente.cnh || '');
  document.getElementById('cliente-email').value = cliente.email || '';
  document.getElementById('cliente-telefone').value = formatarTelefone(cliente.telefone || '');

  const modal = new bootstrap.Modal(document.getElementById('modal-cliente'));
  modal.show();
}

async function salvarCliente(event) {
  event.preventDefault();
  const form = event.target;
  const btnSubmit = form.querySelector('button[type="submit"]');
  const btnOriginalText = btnSubmit.innerHTML;

  const id = document.getElementById('cliente-id').value;
  const clienteData = {
    nome: document.getElementById('cliente-nome').value.trim(),
    cpf: document.getElementById('cliente-cpf').value.trim(),
    cnh: document.getElementById('cliente-cnh').value.trim(),
    email: document.getElementById('cliente-email').value.trim(),
    telefone: document.getElementById('cliente-telefone').value.trim()
  };

  // Validação do número de caracteres
  const digitosCpf = clienteData.cpf.replace(/\D/g, '');
  if (digitosCpf.length !== 11) {
    showToast('O CPF deve conter exatamente 11 dígitos.', 'warning');
    return;
  }

  const digitosTel = clienteData.telefone.replace(/\D/g, '');
  if (digitosTel.length < 10 || digitosTel.length > 11) {
    showToast('O telefone deve conter DDD e 8 ou 9 dígitos.', 'warning');
    return;
  }

  if (clienteData.cnh.length < 8 || clienteData.cnh.length > 11) {
    showToast('A CNH deve conter entre 8 e 11 dígitos numéricos.', 'warning');
    return;
  }

  try {
    btnSubmit.disabled = true;
    btnSubmit.innerHTML = '<span class="spinner-border spinner-border-sm me-1"></span> Salvando...';

    if (id) {
      await ApiService.updateCliente(id, clienteData);
      showToast('Cliente atualizado com sucesso!');
    } else {
      await ApiService.createCliente(clienteData);
      showToast('Cliente cadastrado com sucesso!');
    }

    bootstrap.Modal.getInstance(document.getElementById('modal-cliente')).hide();
    AppState.clientes = await ApiService.getClientes();
    renderClientes();
  } catch (err) {
    showToast(err.message, 'danger');
  } finally {
    btnSubmit.disabled = false;
    btnSubmit.innerHTML = btnOriginalText;
  }
}

async function excluirCliente(id) {
  if (!confirm('Deseja realmente excluir este cliente?')) return;
  try {
    await ApiService.deleteCliente(id);
    showToast('Cliente excluído com sucesso!');
    AppState.clientes = await ApiService.getClientes();
    renderClientes();
  } catch (err) {
    showToast(err.message, 'danger');
  }
}

// --- 3. VEÍCULOS (CRUD + 2 Filtros: Modelo e Placa + Categoria) ---
function renderVeiculos() {
  const filtroModelo = document.getElementById('filtro-veiculo-modelo').value.toLowerCase();
  const filtroPlaca = document.getElementById('filtro-veiculo-placa').value.toLowerCase();
  const filtroCategoria = document.getElementById('filtro-veiculo-categoria').value;

  const filtrados = AppState.veiculos.filter(v => {
    const matchModelo = !filtroModelo || (v.modelo && v.modelo.toLowerCase().includes(filtroModelo));
    const matchPlaca = !filtroPlaca || (v.placa && v.placa.toLowerCase().includes(filtroPlaca));
    const matchCat = !filtroCategoria || (v.categoriaId == filtroCategoria);
    return matchModelo && matchPlaca && matchCat;
  });

  const tbody = document.getElementById('tabela-veiculos-corpo');
  document.getElementById('badge-total-veiculos').textContent = `${filtrados.length} veículo(s)`;

  if (filtrados.length === 0) {
    tbody.innerHTML = '<tr><td colspan="8" class="text-center text-muted py-4">Nenhum veículo encontrado com os filtros aplicados.</td></tr>';
    return;
  }

  tbody.innerHTML = filtrados.map(v => `
    <tr>
      <td class="fw-bold">#${v.id}</td>
      <td>${v.modelo}</td>
      <td><span class="badge bg-secondary font-monospace">${v.placa}</span></td>
      <td>${v.fabricanteNome || '-'}</td>
      <td><span class="badge bg-info text-dark">${v.categoriaNome || '-'}</span></td>
      <td>${v.anoFabricacao || '-'}</td>
      <td>
        <span class="badge ${v.status === 'Disponivel' ? 'bg-success' : (v.status === 'Alugado' ? 'bg-warning text-dark' : 'bg-danger')}">
          ${v.status}
        </span>
      </td>
      <td class="text-end">
        <button class="btn btn-sm btn-outline-primary me-1" onclick="abrirModalEdicaoVeiculo(${v.id})">Editar</button>
        <button class="btn btn-sm btn-outline-danger" onclick="excluirVeiculo(${v.id})">Excluir</button>
      </td>
    </tr>
  `).join('');
}

function popularDropdownsVeiculo() {
  const selFab = document.getElementById('veiculo-fabricante');
  selFab.innerHTML = '<option value="">Selecione...</option>' + 
    AppState.fabricantes.map(f => `<option value="${f.id}">${f.nome}</option>`).join('');

  const selCat = document.getElementById('veiculo-categoria');
  selCat.innerHTML = '<option value="">Selecione...</option>' + 
    AppState.categorias.map(c => `<option value="${c.id}">${c.nome} (Diária: R$ ${c.valorDiariaBase})</option>`).join('');

  const selFiltroCat = document.getElementById('filtro-veiculo-categoria');
  selFiltroCat.innerHTML = '<option value="">Todas as Categorias</option>' + 
    AppState.categorias.map(c => `<option value="${c.id}">${c.nome}</option>`).join('');
}

function abrirModalNovoVeiculo() {
  document.getElementById('veiculo-modal-title').textContent = 'Cadastrar Novo Veículo';
  document.getElementById('form-veiculo').reset();
  document.getElementById('veiculo-id').value = '';
  popularDropdownsVeiculo();
  const modal = new bootstrap.Modal(document.getElementById('modal-veiculo'));
  modal.show();
}

function abrirModalEdicaoVeiculo(id) {
  const veiculo = AppState.veiculos.find(v => v.id === id);
  if (!veiculo) return;

  popularDropdownsVeiculo();
  document.getElementById('veiculo-modal-title').textContent = 'Editar Dados do Veículo';
  document.getElementById('veiculo-id').value = veiculo.id;
  document.getElementById('veiculo-modelo').value = veiculo.modelo;
  document.getElementById('veiculo-placa').value = veiculo.placa;
  document.getElementById('veiculo-ano').value = veiculo.anoFabricacao;
  document.getElementById('veiculo-cor').value = veiculo.cor;
  document.getElementById('veiculo-km').value = veiculo.quilometragem;
  document.getElementById('veiculo-fabricante').value = veiculo.fabricanteId;
  document.getElementById('veiculo-categoria').value = veiculo.categoriaId;

  const modal = new bootstrap.Modal(document.getElementById('modal-veiculo'));
  modal.show();
}

async function salvarVeiculo(event) {
  event.preventDefault();
  const form = event.target;
  const btnSubmit = form.querySelector('button[type="submit"]');
  const btnOriginalText = btnSubmit.innerHTML;

  const id = document.getElementById('veiculo-id').value;
  const veiculoData = {
    modelo: document.getElementById('veiculo-modelo').value.trim(),
    placa: document.getElementById('veiculo-placa').value.trim(),
    anoFabricacao: parseInt(document.getElementById('veiculo-ano').value),
    cor: document.getElementById('veiculo-cor').value.trim(),
    quilometragem: parseInt(document.getElementById('veiculo-km').value),
    fabricanteId: parseInt(document.getElementById('veiculo-fabricante').value),
    categoriaId: parseInt(document.getElementById('veiculo-categoria').value)
  };

  try {
    btnSubmit.disabled = true;
    btnSubmit.innerHTML = '<span class="spinner-border spinner-border-sm me-1"></span> Salvando...';

    if (id) {
      await ApiService.updateVeiculo(id, veiculoData);
      showToast('Veículo atualizado com sucesso!');
    } else {
      await ApiService.createVeiculo(veiculoData);
      showToast('Veículo cadastrado na frota com sucesso!');
    }

    bootstrap.Modal.getInstance(document.getElementById('modal-veiculo')).hide();
    AppState.veiculos = await ApiService.getVeiculos();
    renderVeiculos();
  } catch (err) {
    showToast(err.message, 'danger');
  } finally {
    btnSubmit.disabled = false;
    btnSubmit.innerHTML = btnOriginalText;
  }
}

async function excluirVeiculo(id) {
  if (!confirm('Deseja realmente excluir este veículo da frota?')) return;
  try {
    await ApiService.deleteVeiculo(id);
    showToast('Veículo removido com sucesso!');
    AppState.veiculos = await ApiService.getVeiculos();
    renderVeiculos();
  } catch (err) {
    showToast(err.message, 'danger');
  }
}

// --- 4. ALUGUÉIS & DEVOLUÇÕES (CRUD + 2 Filtros: Cliente e Status) ---
function renderAlugueis() {
  const filtroCliente = document.getElementById('filtro-aluguel-cliente').value.toLowerCase();
  const filtroStatus = document.getElementById('filtro-aluguel-status').value;

  const filtrados = AppState.alugueis.filter(a => {
    const nomeCliente = (a.clienteNome || '').toLowerCase();
    const matchCliente = !filtroCliente || nomeCliente.includes(filtroCliente);
    const matchStatus = !filtroStatus || (a.status === filtroStatus);
    return matchCliente && matchStatus;
  });

  const tbody = document.getElementById('tabela-alugueis-corpo');
  document.getElementById('badge-total-alugueis').textContent = `${filtrados.length} contrato(s)`;

  if (filtrados.length === 0) {
    tbody.innerHTML = '<tr><td colspan="8" class="text-center text-muted py-4">Nenhum contrato de aluguel encontrado com os filtros aplicados.</td></tr>';
    return;
  }

  tbody.innerHTML = filtrados.map(a => `
    <tr>
      <td class="fw-bold">#${a.id}</td>
      <td>${a.clienteNome || 'Cliente #' + a.clienteId}</td>
      <td>${a.veiculoModelo || 'Veículo #' + a.veiculoId} <span class="badge bg-secondary font-monospace">${a.veiculoPlaca || ''}</span></td>
      <td>${new Date(a.dataRetirada).toLocaleDateString('pt-BR')}</td>
      <td>${new Date(a.dataPrevistaDevolucao).toLocaleDateString('pt-BR')}</td>
      <td>
        <span class="badge ${a.status === 'Ativo' ? 'bg-warning text-dark' : 'bg-success'}">
          ${a.status}
        </span>
      </td>
      <td class="fw-bold">${a.valorTotal ? 'R$ ' + Number(a.valorTotal).toFixed(2) : '<span class="text-muted">A calcular</span>'}</td>
      <td class="text-end">
        ${a.status === 'Ativo' ? `
          <button class="btn btn-sm btn-success me-1" onclick="abrirModalDevolucao(${a.id}, ${a.veiculoId})">
            Devolver
          </button>
        ` : ''}
        <button class="btn btn-sm btn-outline-danger" onclick="excluirAluguel(${a.id})">Excluir</button>
      </td>
    </tr>
  `).join('');
}

function abrirModalNovaLocacao() {
  const selCli = document.getElementById('aluguel-cliente');
  selCli.innerHTML = '<option value="">Selecione o Cliente...</option>' + 
    AppState.clientes.map(c => `<option value="${c.id}">${c.nome} (CPF: ${c.cpf})</option>`).join('');

  const selVeic = document.getElementById('aluguel-veiculo');
  const disponiveis = AppState.veiculos.filter(v => v.status === 'Disponivel');
  
  if (disponiveis.length === 0) {
    selVeic.innerHTML = '<option value="">Nenhum veículo disponível no momento!</option>';
  } else {
    selVeic.innerHTML = '<option value="">Selecione o Veículo Disponível...</option>' + 
      disponiveis.map(v => `<option value="${v.id}">${v.modelo} - Placa ${v.placa} (${v.categoriaNome})</option>`).join('');
  }

  // Previsão padrão: 3 dias a partir de hoje
  const hoje = new Date();
  hoje.setDate(hoje.getDate() + 3);
  document.getElementById('aluguel-data-prevista').value = hoje.toISOString().slice(0, 16);

  const modal = new bootstrap.Modal(document.getElementById('modal-aluguel'));
  modal.show();
}

async function salvarAluguel(event) {
  event.preventDefault();
  const form = event.target;
  const btnSubmit = form.querySelector('button[type="submit"]');
  const btnOriginalText = btnSubmit.innerHTML;

  const aluguelData = {
    clienteId: parseInt(document.getElementById('aluguel-cliente').value),
    veiculoId: parseInt(document.getElementById('aluguel-veiculo').value),
    dataPrevistaDevolucao: document.getElementById('aluguel-data-prevista').value
  };

  try {
    btnSubmit.disabled = true;
    btnSubmit.innerHTML = '<span class="spinner-border spinner-border-sm me-1"></span> Registrando...';

    await ApiService.createAluguel(aluguelData);
    showToast('Locação iniciada com sucesso! Veículo alterado para Alugado.');
    bootstrap.Modal.getInstance(document.getElementById('modal-aluguel')).hide();
    
    // Atualizar dados de aluguel e frota
    [AppState.alugueis, AppState.veiculos] = await Promise.all([
      ApiService.getAlugueis(),
      ApiService.getVeiculos()
    ]);
    renderAlugueis();
    renderDashboard();
  } catch (err) {
    showToast(err.message, 'danger');
  } finally {
    btnSubmit.disabled = false;
    btnSubmit.innerHTML = btnOriginalText;
  }
}

function abrirModalDevolucao(aluguelId, veiculoId) {
  const veiculo = AppState.veiculos.find(v => v.id === veiculoId);
  const kmAtual = veiculo ? veiculo.quilometragem : 0;

  document.getElementById('devolucao-aluguel-id').value = aluguelId;
  document.getElementById('devolucao-km-inicial').value = kmAtual;
  document.getElementById('devolucao-km-final').value = kmAtual + 150; // sugestão
  document.getElementById('devolucao-observacoes').value = 'Veículo devolvido em perfeito estado e com tanque cheio.';

  const modal = new bootstrap.Modal(document.getElementById('modal-devolucao'));
  modal.show();
}

async function salvarDevolucao(event) {
  event.preventDefault();
  const form = event.target;
  const btnSubmit = form.querySelector('button[type="submit"]');
  const btnOriginalText = btnSubmit.innerHTML;

  const aluguelId = document.getElementById('devolucao-aluguel-id').value;
  const devolucaoData = {
    quilometragemFinal: parseInt(document.getElementById('devolucao-km-final').value),
    observacoes: document.getElementById('devolucao-observacoes').value.trim()
  };

  try {
    btnSubmit.disabled = true;
    btnSubmit.innerHTML = '<span class="spinner-border spinner-border-sm me-1"></span> Registrando...';

    const res = await ApiService.registrarDevolucao(aluguelId, devolucaoData);
    showToast(`Devolução registrada com sucesso! Total calculado: R$ ${Number(res.valorTotal || 0).toFixed(2)}`);
    bootstrap.Modal.getInstance(document.getElementById('modal-devolucao')).hide();

    [AppState.alugueis, AppState.veiculos] = await Promise.all([
      ApiService.getAlugueis(),
      ApiService.getVeiculos()
    ]);
    renderAlugueis();
    renderDashboard();
  } catch (err) {
    showToast(err.message, 'danger');
  } finally {
    btnSubmit.disabled = false;
    btnSubmit.innerHTML = btnOriginalText;
  }
}

async function excluirAluguel(id) {
  if (!confirm('Deseja realmente cancelar/excluir este registro de locação?')) return;
  try {
    await ApiService.deleteAluguel(id);
    showToast('Locação excluída com sucesso!');
    [AppState.alugueis, AppState.veiculos] = await Promise.all([
      ApiService.getAlugueis(),
      ApiService.getVeiculos()
    ]);
    renderAlugueis();
    renderDashboard();
  } catch (err) {
    showToast(err.message, 'danger');
  }
}

// --- 5. FABRICANTES E CATEGORIAS (CRUDs) ---
function renderFabricantesCategorias() {
  // Filtros de Fabricantes (Nome e País)
  const fNome = document.getElementById('filtro-fab-nome').value.toLowerCase();
  const fPais = document.getElementById('filtro-fab-pais').value.toLowerCase();
  const fabs = AppState.fabricantes.filter(f => {
    return (!fNome || f.nome.toLowerCase().includes(fNome)) &&
           (!fPais || (f.paisOrigem && f.paisOrigem.toLowerCase().includes(fPais)));
  });

  const tbodyFab = document.getElementById('tabela-fabricantes-corpo');
  tbodyFab.innerHTML = fabs.map(f => `
    <tr>
      <td class="fw-bold">#${f.id}</td>
      <td>${f.nome}</td>
      <td>${f.paisOrigem || '-'}</td>
      <td class="text-end">
        <button class="btn btn-sm btn-outline-danger" onclick="excluirFabricante(${f.id})">Excluir</button>
      </td>
    </tr>
  `).join('');

  // Filtros de Categorias (Nome e Diária)
  const cNome = document.getElementById('filtro-cat-nome').value.toLowerCase();
  const cats = AppState.categorias.filter(c => !cNome || c.nome.toLowerCase().includes(cNome));

  const tbodyCat = document.getElementById('tabela-categorias-corpo');
  tbodyCat.innerHTML = cats.map(c => `
    <tr>
      <td class="fw-bold">#${c.id}</td>
      <td>${c.nome}</td>
      <td>${c.descricao || '-'}</td>
      <td class="fw-bold text-success">R$ ${Number(c.valorDiariaBase).toFixed(2)}</td>
      <td class="text-end">
        <button class="btn btn-sm btn-outline-danger" onclick="excluirCategoria(${c.id})">Excluir</button>
      </td>
    </tr>
  `).join('');
}

async function salvarFabricante(event) {
  event.preventDefault();
  const form = event.target;
  const btnSubmit = form.querySelector('button[type="submit"]');
  const btnOriginalText = btnSubmit.innerHTML;

  const fabData = {
    nome: document.getElementById('fab-nome').value.trim(),
    paisOrigem: document.getElementById('fab-pais').value.trim()
  };

  try {
    btnSubmit.disabled = true;
    btnSubmit.innerHTML = '<span class="spinner-border spinner-border-sm me-1"></span> Salvando...';

    await ApiService.createFabricante(fabData);
    showToast('Fabricante cadastrado com sucesso!');
    bootstrap.Modal.getInstance(document.getElementById('modal-fabricante')).hide();
    AppState.fabricantes = await ApiService.getFabricantes();
    renderFabricantesCategorias();
  } catch (err) {
    showToast(err.message, 'danger');
  } finally {
    btnSubmit.disabled = false;
    btnSubmit.innerHTML = btnOriginalText;
  }
}

async function excluirFabricante(id) {
  if (!confirm('Deseja excluir este fabricante?')) return;
  try {
    await ApiService.deleteFabricante(id);
    showToast('Fabricante removido com sucesso!');
    AppState.fabricantes = await ApiService.getFabricantes();
    renderFabricantesCategorias();
  } catch (err) {
    showToast(err.message, 'danger');
  }
}

async function salvarCategoria(event) {
  event.preventDefault();
  const form = event.target;
  const btnSubmit = form.querySelector('button[type="submit"]');
  const btnOriginalText = btnSubmit.innerHTML;

  const catData = {
    nome: document.getElementById('cat-nome').value.trim(),
    descricao: document.getElementById('cat-descricao').value.trim(),
    valorDiariaBase: parseFloat(document.getElementById('cat-diaria').value)
  };

  try {
    btnSubmit.disabled = true;
    btnSubmit.innerHTML = '<span class="spinner-border spinner-border-sm me-1"></span> Salvando...';

    await ApiService.createCategoria(catData);
    showToast('Categoria cadastrada com sucesso!');
    bootstrap.Modal.getInstance(document.getElementById('modal-categoria')).hide();
    AppState.categorias = await ApiService.getCategorias();
    renderFabricantesCategorias();
  } catch (err) {
    showToast(err.message, 'danger');
  } finally {
    btnSubmit.disabled = false;
    btnSubmit.innerHTML = btnOriginalText;
  }
}

async function excluirCategoria(id) {
  if (!confirm('Deseja excluir esta categoria?')) return;
  try {
    await ApiService.deleteCategoria(id);
    showToast('Categoria removida com sucesso!');
    AppState.categorias = await ApiService.getCategorias();
    renderFabricantesCategorias();
  } catch (err) {
    showToast(err.message, 'danger');
  }
}

// --- 6. CONSULTAS AVANÇADAS SETUP ---
function initConsultasView() {
  // Preencher dropdowns do Filtro 1
  const selCat1 = document.getElementById('f1-categoria');
  selCat1.innerHTML = '<option value="">Todas as Categorias</option>' + 
    AppState.categorias.map(c => `<option value="${c.id}">${c.nome}</option>`).join('');

  const selFab1 = document.getElementById('f1-fabricante');
  selFab1.innerHTML = '<option value="">Todos os Fabricantes</option>' + 
    AppState.fabricantes.map(f => `<option value="${f.id}">${f.nome}</option>`).join('');

  // Preencher dropdown de clientes do Filtro 2
  const selCli2 = document.getElementById('f2-cliente');
  selCli2.innerHTML = '<option value="">Selecione um cliente...</option>' + 
    AppState.clientes.map(c => `<option value="${c.id}">${c.nome} (CPF: ${c.cpf})</option>`).join('');

  carregarFiltro1();
  carregarFiltro3();
  carregarFiltro4();
  carregarFiltro5();
}

// Configura listeners para os inputs de filtro das tabelas
function setupFilterListeners() {
  // Clientes
  document.getElementById('filtro-cliente-nome').addEventListener('input', renderClientes);
  document.getElementById('filtro-cliente-cpf').addEventListener('input', renderClientes);

  // Veículos
  document.getElementById('filtro-veiculo-modelo').addEventListener('input', renderVeiculos);
  document.getElementById('filtro-veiculo-placa').addEventListener('input', renderVeiculos);
  document.getElementById('filtro-veiculo-categoria').addEventListener('change', renderVeiculos);

  // Aluguéis
  document.getElementById('filtro-aluguel-cliente').addEventListener('input', renderAlugueis);
  document.getElementById('filtro-aluguel-status').addEventListener('change', renderAlugueis);

  // Fabricantes & Categorias
  document.getElementById('filtro-fab-nome').addEventListener('input', renderFabricantesCategorias);
  document.getElementById('filtro-fab-pais').addEventListener('input', renderFabricantesCategorias);
  document.getElementById('filtro-cat-nome').addEventListener('input', renderFabricantesCategorias);

  // Filtro 3
  document.getElementById('f3-filtro-nome').addEventListener('input', carregarFiltro3);
}

// Configura máscaras de digitação em tempo real
function setupInputMasks() {
  const cpfInput = document.getElementById('cliente-cpf');
  if (cpfInput) {
    cpfInput.addEventListener('input', (e) => {
      e.target.value = formatarCPF(e.target.value);
    });
  }

  const telInput = document.getElementById('cliente-telefone');
  if (telInput) {
    telInput.addEventListener('input', (e) => {
      e.target.value = formatarTelefone(e.target.value);
    });
  }

  const cnhInput = document.getElementById('cliente-cnh');
  if (cnhInput) {
    cnhInput.addEventListener('input', (e) => {
      e.target.value = formatarCNH(e.target.value);
    });
  }

  const filtroCpf = document.getElementById('filtro-cliente-cpf');
  if (filtroCpf) {
    filtroCpf.addEventListener('input', (e) => {
      e.target.value = formatarCPF(e.target.value);
    });
  }
}

