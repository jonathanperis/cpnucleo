namespace Domain.Common.Dtos;

public sealed record WorkflowDto(string? Name, byte Order) : BaseDto;
