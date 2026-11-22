/**
 * Módulo de Integração com a API RESTful - Sistema de Locadora de Veículos
 * Comunicação direta com a API .NET 8 (Backend)
 */

const API_BASE_URL = 'http://localhost:5097/api';

class ApiService {
  static async request(endpoint, options = {}) {
    const url = `${API_BASE_URL}${endpoint}`;
    const defaultHeaders = {
      'Content-Type': 'application/json',
      'Accept': 'application/json'
    };

    const config = {
      ...options,
      headers: {
        ...defaultHeaders,
        ...options.headers
      }
    };

    try {
      const response = await fetch(url, config);

      // Tratamento especial para DELETE ou respostas vazias (204 No Content)
      if (response.status === 204) {
        return null;
      }

      const contentType = response.headers.get('content-type');
      let data = null;
      if (contentType && contentType.includes('application/json')) {
        data = await response.json();
      } else {
        data = await response.text();
      }

      if (!response.ok) {
        let errorMessage = 'Erro ao processar a solicitação no servidor.';
        if (data && typeof data === 'object') {
          if (data.mensagem) {
            errorMessage = data.mensagem;
          } else if (data.errors && typeof data.errors === 'object') {
            const msgs = Object.values(data.errors).flat();
            errorMessage = msgs.join(' ');
          } else if (data.title) {
            errorMessage = data.title;
          } else if (data.detalhe) {
            errorMessage = data.detalhe;
          }
        } else if (typeof data === 'string' && data.trim()) {
          errorMessage = data;
        }
        throw new Error(errorMessage);
      }

      return data;
    } catch (error) {
      console.error(`Erro ao chamar ${url}:`, error);
      if (error.name === 'TypeError' || error.message.includes('Failed to fetch') || error.message.includes('NetworkError')) {
        throw new Error('Não foi possível conectar ao servidor da API. Verifique se o backend está em execução.');
      }
      throw error;
    }
  }

  // --- CLIENTES ---
  static async getClientes() {
    return this.request('/Clientes');
  }

  static async getClienteById(id) {
    return this.request(`/Clientes/${id}`);
  }

  static async createCliente(clienteData) {
    return this.request('/Clientes', {
      method: 'POST',
      body: JSON.stringify(clienteData)
    });
  }

  static async updateCliente(id, clienteData) {
    return this.request(`/Clientes/${id}`, {
      method: 'PUT',
      body: JSON.stringify(clienteData)
    });
  }

  static async deleteCliente(id) {
    return this.request(`/Clientes/${id}`, {
      method: 'DELETE'
    });
  }

  static async getRelatorioLocacoesClientes() {
    return this.request('/Clientes/relatorio-locacoes');
  }

  // --- VEÍCULOS ---
  static async getVeiculos() {
    return this.request('/Veiculos');
  }

  static async getVeiculoById(id) {
    return this.request(`/Veiculos/${id}`);
  }

  static async createVeiculo(veiculoData) {
    return this.request('/Veiculos', {
      method: 'POST',
      body: JSON.stringify(veiculoData)
    });
  }

  static async updateVeiculo(id, veiculoData) {
    return this.request(`/Veiculos/${id}`, {
      method: 'PUT',
      body: JSON.stringify(veiculoData)
    });
  }

  static async deleteVeiculo(id) {
    return this.request(`/Veiculos/${id}`, {
      method: 'DELETE'
    });
  }

  static async getVeiculosDisponiveis(categoriaId = '', fabricanteId = '') {
    const params = new URLSearchParams();
    if (categoriaId) params.append('categoriaId', categoriaId);
    if (fabricanteId) params.append('fabricanteId', fabricanteId);
    const query = params.toString() ? `?${params.toString()}` : '';
    return this.request(`/Veiculos/disponiveis${query}`);
  }

  static async getRelatorioFrota() {
    return this.request('/Veiculos/relatorio-frota');
  }

  // --- FABRICANTES ---
  static async getFabricantes() {
    return this.request('/Fabricantes');
  }

  static async createFabricante(fabricanteData) {
    return this.request('/Fabricantes', {
      method: 'POST',
      body: JSON.stringify(fabricanteData)
    });
  }

  static async updateFabricante(id, fabricanteData) {
    return this.request(`/Fabricantes/${id}`, {
      method: 'PUT',
      body: JSON.stringify(fabricanteData)
    });
  }

  static async deleteFabricante(id) {
    return this.request(`/Fabricantes/${id}`, {
      method: 'DELETE'
    });
  }

  // --- CATEGORIAS ---
  static async getCategorias() {
    return this.request('/Categorias');
  }

  static async createCategoria(categoriaData) {
    return this.request('/Categorias', {
      method: 'POST',
      body: JSON.stringify(categoriaData)
    });
  }

  static async updateCategoria(id, categoriaData) {
    return this.request(`/Categorias/${id}`, {
      method: 'PUT',
      body: JSON.stringify(categoriaData)
    });
  }

  static async deleteCategoria(id) {
    return this.request(`/Categorias/${id}`, {
      method: 'DELETE'
    });
  }

  // --- ALUGUÉIS & DEVOLUÇÕES ---
  static async getAlugueis() {
    return this.request('/Alugueis');
  }

  static async getAluguelById(id) {
    return this.request(`/Alugueis/${id}`);
  }

  static async createAluguel(aluguelData) {
    return this.request('/Alugueis', {
      method: 'POST',
      body: JSON.stringify(aluguelData)
    });
  }

  static async registrarDevolucao(id, devolucaoData) {
    return this.request(`/Alugueis/${id}/devolucao`, {
      method: 'PUT',
      body: JSON.stringify(devolucaoData)
    });
  }

  static async deleteAluguel(id) {
    return this.request(`/Alugueis/${id}`, {
      method: 'DELETE'
    });
  }

  static async getHistoricoCliente(clienteId, status = '') {
    const query = status ? `?status=${encodeURIComponent(status)}` : '';
    return this.request(`/Alugueis/cliente/${clienteId}${query}`);
  }

  static async getAlugueisPorStatusPagamento(statusPagamento = '') {
    const query = statusPagamento ? `?statusPagamento=${encodeURIComponent(statusPagamento)}` : '';
    return this.request(`/Alugueis/status-pagamento${query}`);
  }
}
