namespace GrpcServer.Contracts.Common.Dtos;

public sealed record ProjectDto : BaseDto
{
    public string? Name { get; set; }
    public Guid OrganizationId { get; set; }
    public DateTime? UpdatedAt { get; set; }
}
