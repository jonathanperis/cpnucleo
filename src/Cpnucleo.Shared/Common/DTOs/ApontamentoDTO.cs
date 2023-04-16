namespace Cpnucleo.Shared.Common.Dtos;

public sealed record ApontamentoDto(string? Descricao, DateTime DataApontamento, int QtdHoras, Guid IdTarefa, Guid IdRecurso, TarefaDto? Tarefa) : BaseDto;