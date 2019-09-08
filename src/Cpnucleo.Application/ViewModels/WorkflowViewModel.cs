using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Cpnucleo.Application.ViewModels
{
    public class WorkflowViewModel : BaseViewModel
    {
        [Display(Name = "Nome")]      
        [Required(ErrorMessage = "Necessário informar o {0} do Workflow.")]
        [MaxLength(50, ErrorMessage = "{0} pode conter no máximo {1} caractéres.")]
        public string Nome { get; set; }

        [Display(Name = "Ordem")]      
        [Required(ErrorMessage = "Necessário informar a {0} do Workflow.")]        
        public int? Ordem { get; set; }

        public List<TarefaViewModel> ListaTarefas { get; set; }
    }
}