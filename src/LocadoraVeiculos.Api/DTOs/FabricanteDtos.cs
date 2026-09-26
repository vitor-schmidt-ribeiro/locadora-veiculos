using System.ComponentModel.DataAnnotations;

namespace LocadoraVeiculos.Api.DTOs;

public class FabricanteDto
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string PaisOrigem { get; set; } = string.Empty;
}

public class CriarFabricanteDto
{
    [Required(ErrorMessage = "O nome do fabricante é obrigatório.")]
    [StringLength(100, ErrorMessage = "O nome deve ter no máximo 100 caracteres.")]
    public string Nome { get; set; } = string.Empty;

    [Required(ErrorMessage = "O país de origem é obrigatório.")]
    [StringLength(50, ErrorMessage = "O país de origem deve ter no máximo 50 caracteres.")]
    public string PaisOrigem { get; set; } = string.Empty;
}

public class AtualizarFabricanteDto
{
    [Required(ErrorMessage = "O nome do fabricante é obrigatório.")]
    [StringLength(100, ErrorMessage = "O nome deve ter no máximo 100 caracteres.")]
    public string Nome { get; set; } = string.Empty;

    [Required(ErrorMessage = "O país de origem é obrigatório.")]
    [StringLength(50, ErrorMessage = "O país de origem deve ter no máximo 50 caracteres.")]
    public string PaisOrigem { get; set; } = string.Empty;
}
