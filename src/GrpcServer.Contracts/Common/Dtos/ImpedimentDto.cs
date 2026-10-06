namespace GrpcServer.Contracts.Common.Dtos;

public sealed record ImpedimentDto : BaseDto
{
    public string? Name { get; set; }
}
