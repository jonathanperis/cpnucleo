namespace GrpcServer.Contracts.Common.Dtos;

public sealed record AssignmentImpedimentDto : BaseDto
{
    public string? Description { get; set; }
    public Guid AssignmentId { get; set; }
    public Guid ImpedimentId { get; set; }
}
