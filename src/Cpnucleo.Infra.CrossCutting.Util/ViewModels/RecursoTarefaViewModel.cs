namespace Cpnucleo.Infra.CrossCutting.Util.ViewModels;

public class RecursoTarefaViewModel : BaseViewModel
{
    [Display(Name = "Recurso")]
    [Required(ErrorMessage = "Necessário informar o {0}.")]
    public Guid IdRecurso { get; set; }

    [Display(Name = "Tarefa")]
    [Required(ErrorMessage = "Necessário informar o {0}.")]
    public Guid IdTarefa { get; set; }

    public RecursoViewModel? Recurso { get; set; }

    public TarefaViewModel? Tarefa { get; set; }
}