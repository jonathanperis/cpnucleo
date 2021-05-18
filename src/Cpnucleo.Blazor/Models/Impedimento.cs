using System.ComponentModel.DataAnnotations;

namespace Cpnucleo.Blazor.Models
{
    public class Impedimento : Base
    {
        [Display(Name = "Nome")]
        [Required(ErrorMessage = "Necessário informar o {0} do Impedimento.")]
        [MaxLength(50, ErrorMessage = "{0} pode conter no máximo {1} caractéres.")]
        public string Nome { get; set; }
    }
}