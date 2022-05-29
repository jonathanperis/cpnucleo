namespace Cpnucleo.Infra.CrossCutting.Util.Common.DTOs;

public class RecursoTarefaDTO : BaseDTO
{
    [Display(Name = "Recurso")]
    [Required(ErrorMessage = "Necessário informar o {0}.")]
    public Guid IdRecurso { get; set; }

    [Display(Name = "Tarefa")]
    [Required(ErrorMessage = "Necessário informar o {0}.")]
    public Guid IdTarefa { get; set; }

    public RecursoDTO? Recurso { get; set; }

    public TarefaDTO? Tarefa { get; set; }
}