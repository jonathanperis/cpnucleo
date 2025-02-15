namespace Domain.Common.Dtos;

public sealed record UserProjectDto(Ulid UserId, Ulid ProjectId) : BaseDto;
