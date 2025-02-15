namespace Domain.Common.Dtos;

public sealed record AssignmentImpedimentDto(string? Description, Ulid AssignmentId, Ulid ImpedimentId) : BaseDto;
