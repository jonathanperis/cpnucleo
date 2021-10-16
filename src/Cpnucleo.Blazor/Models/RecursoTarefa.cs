using System.ComponentModel.DataAnnotations;

namespace Cpnucleo.Blazor.Models
{
    public class RecursoTarefa : Base
    {
        [Display(Name = "Recurso")]
        [Required(ErrorMessage = "Necessário informar o {0}.")]
        public Guid IdRecurso { get; set; }

        [Display(Name = "Tarefa")]
        [Required(ErrorMessage = "Necessário informar o {0}.")]
        public Guid IdTarefa { get; set; }

        public Recurso Recurso { get; set; }

        public Tarefa Tarefa { get; set; }
    }
}