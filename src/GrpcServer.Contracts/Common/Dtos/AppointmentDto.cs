namespace GrpcServer.Contracts.Common.Dtos;

public sealed record AppointmentDto : BaseDto
{
    public string? Description { get; set; }
    public DateTime KeepDate { get; set; }
    public int AmountHours { get; set; }
    public Guid AssignmentId { get; set; }
    public Guid UserId { get; set; }
}
