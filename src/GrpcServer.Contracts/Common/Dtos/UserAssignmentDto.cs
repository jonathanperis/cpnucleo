namespace GrpcServer.Contracts.Common.Dtos;

public sealed record UserAssignmentDto : BaseDto
{
    public Guid UserId { get; set; }
    public Guid AssignmentId { get; set; }
}
