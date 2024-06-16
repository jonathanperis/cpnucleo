namespace Application.Common.Dtos;

public sealed record ProjectDto(string? Name, Ulid SystemId) : BaseDto;
