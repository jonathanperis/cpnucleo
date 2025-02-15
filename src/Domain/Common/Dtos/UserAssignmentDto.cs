namespace Domain.Common.Dtos;

public sealed record UserAssignmentDto(Ulid UserId, Ulid AssignmentId) : BaseDto;
