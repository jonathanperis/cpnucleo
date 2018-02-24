using System.ComponentModel.DataAnnotations;

namespace dotnet_cpnucleo_pages.Repository.HomeViewModel
{
    public class HomeItem
    {
        public int IdRecurso { get; set; }

        [Display(Name = "Login")]
        [Required(ErrorMessage = "Necessário informar o {0}.")]
        public string Login { get; set; }

        [Display(Name = "Senha")]
        [Required(ErrorMessage = "Necessário informar a {0}.")]
        public string Senha { get; set; }
    }
}