namespace WebApi.Common.Dtos;

public sealed record WorkflowDto : BaseDto
{
    public string? Name { get; set; }
    public int Order { get; set; }
}
