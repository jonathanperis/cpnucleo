namespace GrpcServer.Contracts.Common.Dtos;

public sealed record UserProjectDto : BaseDto
{
    public Guid UserId { get; set; }
    public Guid ProjectId { get; set; }
}
