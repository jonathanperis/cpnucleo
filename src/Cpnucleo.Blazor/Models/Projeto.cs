using System.ComponentModel.DataAnnotations;

namespace Cpnucleo.Blazor.Models
{
    public class Projeto : Base
    {
        [Display(Name = "Nome")]
        [Required(ErrorMessage = "Necessário informar o {0} do Projeto.")]
        [MaxLength(50, ErrorMessage = "{0} pode conter no máximo {1} caractéres.")]
        public string Nome { get; set; }

        [Required]
        [Display(Name = "Sistema")]
        public Guid IdSistema { get; set; }

        public Sistema Sistema { get; set; }
    }
}