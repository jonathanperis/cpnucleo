namespace Cpnucleo.Shared.Common.Dtos;

public sealed record ImpedimentoTarefaDto : BaseDto
{
    public string? Descricao { get; set; }

    public Guid IdTarefa { get; set; }

    public Guid IdImpedimento { get; set; }

    public TarefaDto? Tarefa { get; set; }

    public ImpedimentoDto? Impedimento { get; set; }
}