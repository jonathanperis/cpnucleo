namespace Cpnucleo.Shared.Common.Dtos;

public sealed record ApontamentoDto : BaseDto
{
    public string? Descricao { get; set; }

    public DateTime DataApontamento { get; set; }

    public int QtdHoras { get; set; }

    public Guid IdTarefa { get; set; }

    public Guid IdRecurso { get; set; }

    public TarefaDto? Tarefa { get; set; }
}