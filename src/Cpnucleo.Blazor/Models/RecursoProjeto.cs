using System.ComponentModel.DataAnnotations;

namespace Cpnucleo.Blazor.Models
{
    public class RecursoProjeto : Base
    {
        [Display(Name = "Recurso")]
        [Required(ErrorMessage = "Necessário informar o {0}.")]
        public Guid IdRecurso { get; set; }

        [Display(Name = "Projeto")]
        [Required(ErrorMessage = "Necessário informar o {0}.")]
        public Guid IdProjeto { get; set; }

        public Recurso Recurso { get; set; }

        public Projeto Projeto { get; set; }
    }
}