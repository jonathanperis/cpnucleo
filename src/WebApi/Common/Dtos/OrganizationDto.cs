namespace WebApi.Common.Dtos;

public sealed record OrganizationDto : BaseDto
{
    public string? Name { get; set; }
    public string? Description { get; set; }
}
