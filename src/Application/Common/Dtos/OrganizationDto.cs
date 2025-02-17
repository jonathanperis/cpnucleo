namespace Application.Common.Dtos;

public sealed record OrganizationDto : BaseDto
{
    public string? Name { get; set; }
    public string? Description { get; set; }

    public static implicit operator OrganizationDto(Organization entity)
    {
        var dto = new OrganizationDto()
        {
            Id = entity.Id,
            CreatedAt = entity.CreatedAt
        };
        dto.Name = entity.Name;
        dto.Description = entity.Description;
        return dto;
    }

    public static implicit operator Organization(OrganizationDto dto)
    {
        return Organization.Create(dto.Name, dto.Description, dto.Id);
    }
}