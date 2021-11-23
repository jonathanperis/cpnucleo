using MessagePack;

namespace Cpnucleo.Infra.CrossCutting.Util.ViewModels;

[MessagePackObject(true)]
public class AuthViewModel
{
    [Display(Name = "Login")]
    [Required(ErrorMessage = "Necessário informar o {0}.")]
    public string Usuario { get; set; }

    [Display(Name = "Senha")]
    [DataType(DataType.Password)]
    [Required(ErrorMessage = "Necessário informar a {0}.")]
    public string Senha { get; set; }
}