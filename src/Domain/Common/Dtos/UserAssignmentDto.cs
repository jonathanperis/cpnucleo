namespace Domain.Common.Dtos;

public sealed record UserAssignmentDto : BaseDto
{
    public Ulid UserId { get; set; }
    public Ulid AssignmentId { get; set; }

    public static implicit operator UserAssignmentDto(UserAssignment entity)
    {
        return new()
        {
            Id = entity.Id,
            CreatedAt = entity.CreatedAt,
            UserId = entity.UserId,
            AssignmentId = entity.AssignmentId
        };
    }

    public static implicit operator UserAssignment(UserAssignmentDto dto)
    {
        return UserAssignment.Create(
            dto.UserId,
            dto.AssignmentId,
            dto.Id
        );
    }
}
