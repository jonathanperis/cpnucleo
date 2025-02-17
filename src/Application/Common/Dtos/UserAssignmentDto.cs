namespace Application.Common.Dtos;

public sealed record UserAssignmentDto : BaseDto
{
    public Guid UserId { get; set; }
    public Guid AssignmentId { get; set; }

    // public static implicit operator UserAssignmentDto?(UserAssignment? entity)
    // {
    //     if (entity is null)
    //     {
    //         return null;
    //     }
        
    //     var dto = new UserAssignmentDto
    //     {
    //         Id = entity.Id,
    //         CreatedAt = entity.CreatedAt,
    //         UserId = entity.UserId,
    //         AssignmentId = entity.AssignmentId
    //     };
        
    //     return dto;
    // }

    // public static implicit operator UserAssignment(UserAssignmentDto dto)
    // {
    //     return UserAssignment.Create(
    //         dto.UserId,
    //         dto.AssignmentId,
    //         dto.Id
    //     );
    // }
}
