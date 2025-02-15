namespace Domain.Common.Dtos;

public sealed record OrganizationDto(string? Name, string? Description) : BaseDto;