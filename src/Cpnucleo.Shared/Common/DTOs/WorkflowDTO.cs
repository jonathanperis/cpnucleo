namespace Cpnucleo.Shared.Common.Dtos;

public sealed record WorkflowDto : BaseDto
{
    public string? Nome { get; set; }

    public int Ordem { get; set; }

    public string? TamanhoColuna { get; set; }
}