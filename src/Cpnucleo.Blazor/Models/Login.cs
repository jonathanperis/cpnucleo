using System.ComponentModel.DataAnnotations;

namespace Cpnucleo.Blazor.Models
{
    public class Login
    {
        [Display(Name = "Login")]
        [Required(ErrorMessage = "Necessário informar o {0}.")]
        public string Usuario { get; set; }

        [Display(Name = "Senha")]
        [Required(ErrorMessage = "Necessário informar a {0}.")]
        public string Senha { get; set; }
    }
}