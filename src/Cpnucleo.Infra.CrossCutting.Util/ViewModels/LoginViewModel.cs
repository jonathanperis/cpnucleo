namespace Cpnucleo.Infra.CrossCutting.Util.ViewModels
{
    [DataContract]
    public class LoginViewModel
    {
        [Display(Name = "Login")]
        [Required(ErrorMessage = "Necessário informar o {0}.")]
        [DataMember(Order = 1)]
        public string Usuario { get; set; }

        [Display(Name = "Senha")]
        [Required(ErrorMessage = "Necessário informar a {0}.")]
        [DataMember(Order = 2)]
        public string Senha { get; set; }
    }
}