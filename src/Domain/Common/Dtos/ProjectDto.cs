namespace Domain.Common.Dtos;

public sealed record ProjectDto : BaseDto
{
    public string? Name { get; set; }
    public Ulid OrganizationId { get; set; }

    public static implicit operator ProjectDto(Project entity)
    {
        return new ProjectDto
        {
            Id = entity.Id,
            CreatedAt = entity.CreatedAt,
            Name = entity.Name,
            OrganizationId = entity.OrganizationId
        };
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
