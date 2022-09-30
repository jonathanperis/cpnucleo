namespace Cpnucleo.Shared.Common.DTOs;

public sealed record ImpedimentoDTO : BaseDTO
{
    [Display(Name = "Nome")]
    [Required(ErrorMessage = "Necess�rio informar o {0} do Impedimento.")]
    [MaxLength(50, ErrorMessage = "{0} pode conter no m�ximo {1} caract�res.")]
    public string Nome { get; set; }
}