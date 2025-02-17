namespace Application.Common.Dtos;

public sealed record AssignmentDto : BaseDto
{
    public string? Name { get; set; }
    public string? Description { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public byte AmountHours { get; set; }
    public Guid ProjectId { get; set; }
    public Guid WorkflowId { get; set; }
    public Guid UserId { get; set; }
    public Guid AssignmentTypeId { get; set; }

    public static implicit operator AssignmentDto(Assignment entity)
    {
        var dto = new AssignmentDto
        {
            Id = entity.Id,
            CreatedAt = entity.CreatedAt
        };
        dto.Name = entity.Name;
        dto.Description = entity.Description;
        dto.StartDate = entity.StartDate;
        dto.EndDate = entity.EndDate;
        dto.AmountHours = entity.AmountHours;
        dto.ProjectId = entity.ProjectId;
        dto.WorkflowId = entity.WorkflowId;
        dto.UserId = entity.UserId;
        dto.AssignmentTypeId = entity.AssignmentTypeId;
        return dto;
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
