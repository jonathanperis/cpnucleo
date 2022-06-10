namespace Cpnucleo.Infra.CrossCutting.Util.Common.DTOs;

public class RecursoDTO : BaseDTO
{
    [Display(Name = "Nome")]
    [Required(ErrorMessage = "Necess�rio informar o {0} do Recurso.")]
    [MaxLength(50, ErrorMessage = "{0} pode conter no m�ximo {1} caract�res.")]
    public string Nome { get; set; }

    [Display(Name = "Login")]
    [Required(ErrorMessage = "Necess�rio informar o {0} do Recurso.")]
    [MaxLength(50, ErrorMessage = "{0} pode conter no m�ximo {1} caract�res.")]
    public string Login { get; set; }

    [Display(Name = "Senha")]
    [DataType(DataType.Password)]
    [Required(ErrorMessage = "Necess�rio informar a {0} do Recurso.")]
    [StringLength(50, ErrorMessage = "A {0} deve conter ao menos {2} e o m�ximo {1} caract�res.", MinimumLength = 8)]
    public string? Senha { get; set; }

    [Display(Name = "Confirma��o de Senha")]
    [DataType(DataType.Password)]
    [Required(ErrorMessage = "Necess�rio informar a {0} do Recurso.")]
    [Compare("Senha", ErrorMessage = "A {1} e a {0} n�o correspondem.")]
    public string? ConfirmarSenha { get; set; }
}