using System.ComponentModel.DataAnnotations;

namespace Cpnucleo.Application.ViewModels
{
    public class SistemaViewModel : BaseViewModel
    {
        [Display(Name = "Nome")]      
        [Required(ErrorMessage = "Necessário informar o {0} do Sistema.")]
        [MaxLength(50, ErrorMessage = "{0} pode conter no máximo {1} caractéres.")]
        public string Nome { get; set; }

        [Display(Name = "Descrição")]
        [Required(ErrorMessage = "Necessário informar a {0} do Sistema.")]
        [MaxLength(450, ErrorMessage = "{0} pode conter no máximo {1} caractéres.")]
        public string Descricao { get; set; }        
    }
}