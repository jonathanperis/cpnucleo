using System.ComponentModel.DataAnnotations;

namespace Cpnucleo.Application.ViewModels
{
    public class TipoTarefaViewModel : BaseViewModel
    {
        [Display(Name = "Nome")]      
        [Required(ErrorMessage = "Necessário informar o {0} do Tipo Tarefa.")]
        [MaxLength(50, ErrorMessage = "{0} pode conter no máximo {1} caractéres.")]
        public string Nome { get; set; }
    }
}