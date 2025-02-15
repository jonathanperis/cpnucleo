namespace Domain.Common.Dtos;

public sealed record ProjectDto(string? Name, Ulid OrganizationId) : BaseDto;
