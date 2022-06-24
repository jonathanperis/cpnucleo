namespace Cpnucleo.Infra.CrossCutting.Shared.Common.DTOs;

public class SistemaDTO : BaseDTO
{
    [Display(Name = "Nome")]
    [Required(ErrorMessage = "Necess�rio informar o {0} do Sistema.")]
    [MaxLength(50, ErrorMessage = "{0} pode conter no m�ximo {1} caract�res.")]
    public string Nome { get; set; }

    [Display(Name = "Descri��o")]
    [Required(ErrorMessage = "Necess�rio informar a {0} do Sistema.")]
    [MaxLength(450, ErrorMessage = "{0} pode conter no m�ximo {1} caract�res.")]
    public string Descricao { get; set; }
}