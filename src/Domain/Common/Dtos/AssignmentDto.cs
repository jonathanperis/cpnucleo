namespace Domain.Common.Dtos;

public sealed record AssignmentDto : BaseDto
{
    public string? Name { get; set; }
    public string? Description { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public byte AmountHours { get; set; }
    public Ulid ProjectId { get; set; }
    public Ulid WorkflowId { get; set; }
    public Ulid UserId { get; set; }
    public Ulid AssignmentTypeId { get; set; }

    public static implicit operator AssignmentDto(Assignment entity)
    {
        return new AssignmentDto
        {
            Id = entity.Id,
            CreatedAt = entity.CreatedAt,
            Name = entity.Name,
            Description = entity.Description,
            StartDate = entity.StartDate,
            EndDate = entity.EndDate,
            AmountHours = entity.AmountHours,
            ProjectId = entity.ProjectId,
            WorkflowId = entity.WorkflowId,
            UserId = entity.UserId,
            AssignmentTypeId = entity.AssignmentTypeId
        };
    }

    public static implicit operator Assignment(AssignmentDto dto)
    {
        return Assignment.Create(
            dto.Name,
            dto.Description,
            dto.StartDate,
            dto.EndDate,
            dto.AmountHours,
            dto.ProjectId,
            dto.WorkflowId,
            dto.UserId,
            dto.AssignmentTypeId,
            dto.Id
        );
    }
}
