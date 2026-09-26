using System.ComponentModel.DataAnnotations;
using LocadoraVeiculos.Domain.Enums;

namespace LocadoraVeiculos.Api.DTOs;

public class VeiculoDto
{
    public int Id { get; set; }
    public string Modelo { get; set; } = string.Empty;
    public int AnoFabricacao { get; set; }
    public int Quilometragem { get; set; }
    public string Placa { get; set; } = string.Empty;
    public string Cor { get; set; } = string.Empty;
    public StatusVeiculo Status { get; set; }
    public int FabricanteId { get; set; }
    public string FabricanteNome { get; set; } = string.Empty;
    public int CategoriaId { get; set; }
    public string CategoriaNome { get; set; } = string.Empty;
}

public class CriarVeiculoDto
{
    [Required(ErrorMessage = "O modelo é obrigatório.")]
    [StringLength(100, ErrorMessage = "O modelo deve ter no máximo 100 caracteres.")]
    public string Modelo { get; set; } = string.Empty;

    [Required(ErrorMessage = "O ano de fabricação é obrigatório.")]
    [Range(1950, 2100, ErrorMessage = "Ano de fabricação inválido.")]
    public int AnoFabricacao { get; set; }

    [Required(ErrorMessage = "A quilometragem é obrigatória.")]
    [Range(0, 2000000, ErrorMessage = "Quilometragem inválida.")]
    public int Quilometragem { get; set; }

    [Required(ErrorMessage = "A placa é obrigatória.")]
    [StringLength(10, ErrorMessage = "A placa deve ter no máximo 10 caracteres.")]
    public string Placa { get; set; } = string.Empty;

    [Required(ErrorMessage = "A cor é obrigatória.")]
    [StringLength(30, ErrorMessage = "A cor deve ter no máximo 30 caracteres.")]
    public string Cor { get; set; } = string.Empty;

    [Required(ErrorMessage = "O fabricante é obrigatório.")]
    public int FabricanteId { get; set; }

    [Required(ErrorMessage = "A categoria é obrigatória.")]
    public int CategoriaId { get; set; }
}

public class AtualizarVeiculoDto
{
    [Required(ErrorMessage = "O modelo é obrigatório.")]
    [StringLength(100, ErrorMessage = "O modelo deve ter no máximo 100 caracteres.")]
    public string Modelo { get; set; } = string.Empty;

    [Required(ErrorMessage = "O ano de fabricação é obrigatório.")]
    [Range(1950, 2100, ErrorMessage = "Ano de fabricação inválido.")]
    public int AnoFabricacao { get; set; }

    [Required(ErrorMessage = "A quilometragem é obrigatória.")]
    [Range(0, 2000000, ErrorMessage = "Quilometragem inválida.")]
    public int Quilometragem { get; set; }

    [Required(ErrorMessage = "A placa é obrigatória.")]
    [StringLength(10, ErrorMessage = "A placa deve ter no máximo 10 caracteres.")]
    public string Placa { get; set; } = string.Empty;

    [Required(ErrorMessage = "A cor é obrigatória.")]
    [StringLength(30, ErrorMessage = "A cor deve ter no máximo 30 caracteres.")]
    public string Cor { get; set; } = string.Empty;

    [Required(ErrorMessage = "O status do veículo é obrigatório.")]
    public StatusVeiculo Status { get; set; }

    [Required(ErrorMessage = "O fabricante é obrigatório.")]
    public int FabricanteId { get; set; }

    [Required(ErrorMessage = "A categoria é obrigatória.")]
    public int CategoriaId { get; set; }
}

public class VeiculoDisponivelDto
{
    public int VeiculoId { get; set; }
    public string Modelo { get; set; } = string.Empty;
    public int AnoFabricacao { get; set; }
    public string Placa { get; set; } = string.Empty;
    public string Cor { get; set; } = string.Empty;
    public int Quilometragem { get; set; }
    public string Fabricante { get; set; } = string.Empty;
    public string PaisFabricante { get; set; } = string.Empty;
    public string Categoria { get; set; } = string.Empty;
    public decimal ValorDiariaBase { get; set; }
}

public class RelatorioFrotaDto
{
    public int VeiculoId { get; set; }
    public string Modelo { get; set; } = string.Empty;
    public string Placa { get; set; } = string.Empty;
    public string Fabricante { get; set; } = string.Empty;
    public string Categoria { get; set; } = string.Empty;
    public StatusVeiculo StatusAtual { get; set; }
    public int TotalLocacoes { get; set; }
    public decimal TotalFaturado { get; set; }
    public int? UltimoKmRegistrado { get; set; }
}
