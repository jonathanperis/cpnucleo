namespace Cpnucleo.Shared.Common.DTOs;

public sealed record ProjetoDTO : BaseDTO
{
    [Display(Name = "Nome")]
    [Required(ErrorMessage = "Necessário informar o {0} do Projeto.")]
    [MaxLength(50, ErrorMessage = "{0} pode conter no máximo {1} caractéres.")]
    public string Nome { get; set; }

    [Required]
    [Display(Name = "Sistema")]
    public Guid IdSistema { get; set; }

    public SistemaDTO? Sistema { get; set; }
}