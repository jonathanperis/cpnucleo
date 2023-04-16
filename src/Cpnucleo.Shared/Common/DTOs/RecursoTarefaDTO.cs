namespace Cpnucleo.Shared.Common.Dtos;

public sealed record RecursoTarefaDto : BaseDto
{
    public Guid IdRecurso { get; set; }

    public Guid IdTarefa { get; set; }

    public RecursoDto? Recurso { get; set; }

    public TarefaDto? Tarefa { get; set; }
}