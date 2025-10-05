namespace WebApi.Common.Dtos;

public sealed record AssignmentDto : BaseDto
{
    public string? Name { get; set; }
    public string? Description { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public int AmountHours { get; set; }
    public Guid ProjectId { get; set; }
    public Guid WorkflowId { get; set; }
    public Guid UserId { get; set; }
    public Guid AssignmentTypeId { get; set; }

    // public static implicit operator AssignmentDto?(Assignment? entity)
    // {
    //     if (entity is null)
    //     {
    //         return null;
    //     }

    //     var dto = new AssignmentDto
    //     {
    //         Id = entity.Id,
    //         CreatedAt = entity.CreatedAt,
    //         Name = entity.Name,
    //         Description = entity.Description,
    //         StartDate = entity.StartDate,
    //         EndDate = entity.EndDate,
    //         AmountHours = entity.AmountHours,
    //         ProjectId = entity.ProjectId,
    //         WorkflowId = entity.WorkflowId,
    //         UserId = entity.UserId,
    //         AssignmentTypeId = entity.AssignmentTypeId
    //     };

    //     return dto;
    // }

    // public static implicit operator Assignment(AssignmentDto dto)
    // {
    //     return Assignment.Create(
    //         dto.Name,
    //         dto.Description,
    //         dto.StartDate,
    //         dto.EndDate,
    //         dto.AmountHours,
    //         dto.ProjectId,
    //         dto.WorkflowId,
    //         dto.UserId,
    //         dto.AssignmentTypeId,
    //         dto.Id
    //     );
    // }
}
