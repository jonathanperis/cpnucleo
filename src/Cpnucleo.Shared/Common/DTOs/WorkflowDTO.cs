namespace Cpnucleo.Shared.Common.Dtos;

public sealed record WorkflowDto(string? Nome, int Ordem, string? TamanhoColuna) : BaseDto;