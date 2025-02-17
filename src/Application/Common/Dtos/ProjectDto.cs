namespace Application.Common.Dtos;

public sealed record ProjectDto : BaseDto
{
    public string? Name { get; set; }
    public Guid OrganizationId { get; set; }

    public static implicit operator ProjectDto?(Project? entity)
    {
        if (entity is null)
        {
            return null;
        }
        
        var dto = new ProjectDto
        {
            Id = entity.Id,
            CreatedAt = entity.CreatedAt,
            Name = entity.Name,
            OrganizationId = entity.OrganizationId
        };
        
        return dto;
    }

    public static implicit operator Project(ProjectDto dto)
    {
        return Project.Create(
            dto.Name,
            dto.OrganizationId,
            dto.Id
        );
    }
}
