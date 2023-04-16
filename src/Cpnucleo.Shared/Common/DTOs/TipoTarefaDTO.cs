namespace Cpnucleo.Shared.Common.Dtos;

public sealed record TipoTarefaDto : BaseDto
{
    public string? Nome { get; set; }

    public string? Element { get; set; }

    public string? Image { get; set; }
}