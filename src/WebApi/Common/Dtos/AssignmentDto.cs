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
}
