namespace Cpnucleo.Infra.CrossCutting.Util.Common.DTOs;

public class ImpedimentoDTO : BaseDTO
{
    [Display(Name = "Nome")]
    [Required(ErrorMessage = "Necess�rio informar o {0} do Impedimento.")]
    [MaxLength(50, ErrorMessage = "{0} pode conter no m�ximo {1} caract�res.")]
    public string Nome { get; init; }
}