namespace Application.Common.Dtos;

public sealed record OrganizationDto(string? Name, string? Description) : BaseDto;