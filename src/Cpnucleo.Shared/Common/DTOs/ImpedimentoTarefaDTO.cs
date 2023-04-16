namespace Cpnucleo.Shared.Common.Dtos;

public sealed record ImpedimentoTarefaDto(string? Descricao, Guid IdTarefa, Guid IdImpedimento, TarefaDto? Tarefa, ImpedimentoDto? Impedimento) : BaseDto;