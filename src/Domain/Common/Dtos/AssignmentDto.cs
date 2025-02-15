namespace Domain.Common.Dtos;

public sealed record AssignmentDto(string? Name, string? Description, DateTime StartDate, DateTime EndDate, byte AmountHours, Ulid ProjectId, Ulid WorkflowId, Ulid UserId, Ulid AssignmentTypeId) : BaseDto;
