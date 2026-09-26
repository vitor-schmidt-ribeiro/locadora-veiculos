using System.ComponentModel.DataAnnotations;

namespace LocadoraVeiculos.Api.DTOs;

public class CategoriaDto
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string? Descricao { get; set; }
    public decimal ValorDiariaBase { get; set; }
}

public class CriarCategoriaDto
{
    [Required(ErrorMessage = "O nome da categoria é obrigatório.")]
    [StringLength(50, ErrorMessage = "O nome deve ter no máximo 50 caracteres.")]
    public string Nome { get; set; } = string.Empty;

    [StringLength(250, ErrorMessage = "A descrição deve ter no máximo 250 caracteres.")]
    public string? Descricao { get; set; }

    [Required(ErrorMessage = "O valor da diária base é obrigatório.")]
    [Range(0.01, 100000.00, ErrorMessage = "O valor da diária deve ser maior que zero.")]
    public decimal ValorDiariaBase { get; set; }
}

public class AtualizarCategoriaDto
{
    [Required(ErrorMessage = "O nome da categoria é obrigatório.")]
    [StringLength(50, ErrorMessage = "O nome deve ter no máximo 50 caracteres.")]
    public string Nome { get; set; } = string.Empty;

    [StringLength(250, ErrorMessage = "A descrição deve ter no máximo 250 caracteres.")]
    public string? Descricao { get; set; }

    [Required(ErrorMessage = "O valor da diária base é obrigatório.")]
    [Range(0.01, 100000.00, ErrorMessage = "O valor da diária deve ser maior que zero.")]
    public decimal ValorDiariaBase { get; set; }
}
