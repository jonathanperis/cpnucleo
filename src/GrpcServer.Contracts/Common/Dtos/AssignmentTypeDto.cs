namespace GrpcServer.Contracts.Common.Dtos;

public sealed record AssignmentTypeDto : BaseDto
{
    public string? Name { get; set; }
}
