namespace Cpnucleo.Shared.Common.Dtos;

public sealed record SistemaDto : BaseDto
{
    public string? Nome { get; set; }

    public string? Descricao { get; set; }
}