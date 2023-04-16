namespace Cpnucleo.Shared.Common.Dtos;

public sealed record RecursoTarefaDto : BaseDto
{
    [Display(Name = "Recurso")]
    [Required(ErrorMessage = "Necess�rio informar o {0}.")]
    public Guid IdRecurso { get; set; }

    [Display(Name = "Tarefa")]
    [Required(ErrorMessage = "Necess�rio informar o {0}.")]
    public Guid IdTarefa { get; set; }

    public RecursoDto? Recurso { get; set; }

    public TarefaDto? Tarefa { get; set; }
}