using System.ComponentModel.DataAnnotations;

namespace dotnet_cpnucleo_pages.Repository.Login
{
    public class LoginItem
    {
        [Display(Name = "Login")]
        [Required(ErrorMessage = "Necessário informar o {0}.")]
        public string Usuario { get; set; }

        [Display(Name = "Senha")]
        [Required(ErrorMessage = "Necessário informar a {0}.")]
        public string Senha { get; set; }
    }
}