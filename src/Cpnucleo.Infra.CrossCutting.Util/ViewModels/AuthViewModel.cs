namespace Cpnucleo.Infra.CrossCutting.Util.ViewModels;

public class AuthViewModel
{
    [Display(Name = "Login")]
    [Required(ErrorMessage = "Necessário informar o {0} do Recurso.")]
    [DataMember(Order = 4)]
    public string Login { get; set; }

    [Display(Name = "Senha")]
    [DataType(DataType.Password)]
    [Required(ErrorMessage = "Necessário informar a {0} do Recurso.")]
    [DataMember(Order = 5)]
    public string Senha { get; set; }
}