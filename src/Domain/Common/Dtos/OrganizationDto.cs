namespace Domain.Common.Dtos;

public sealed record OrganizationDto : BaseDto
{
    public string? Name { get; set; }
    public string? Description { get; set; }

    public static implicit operator OrganizationDto(Organization entity)
    {
        return new()
        {
            Id = entity.Id,
            CreatedAt = entity.CreatedAt,
            Name = entity.Name,
            Description = entity.Description
        };
    }

    public static implicit operator Organization(OrganizationDto dto)
    {
        return Organization.Create(dto.Name, dto.Description, dto.Id);
    }
}