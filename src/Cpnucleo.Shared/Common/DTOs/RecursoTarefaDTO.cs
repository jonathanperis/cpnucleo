namespace Cpnucleo.Shared.Common.Dtos;

public sealed record RecursoTarefaDto(Guid IdRecurso, Guid IdTarefa, RecursoDto? Recurso, TarefaDto? Tarefa) : BaseDto;