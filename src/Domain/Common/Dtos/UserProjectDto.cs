namespace Domain.Common.Dtos;

public sealed record UserProjectDto : BaseDto
{
    public Ulid UserId { get; set; }
    public Ulid ProjectId { get; set; }

    public static implicit operator UserProjectDto(UserProject entity)
    {
        return new()
        {
            Id = entity.Id,
            CreatedAt = entity.CreatedAt,
            UserId = entity.UserId,
            ProjectId = entity.ProjectId
        };
    }

    public static implicit operator UserProject(UserProjectDto dto)
    {
        return UserProject.Create(
            dto.UserId,
            dto.ProjectId,
            dto.Id
        );
    }
}
