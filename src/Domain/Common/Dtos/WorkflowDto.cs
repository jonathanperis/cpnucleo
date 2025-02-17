namespace Domain.Common.Dtos;

public sealed record WorkflowDto : BaseDto
{
    public string? Name { get; set; }
    public byte Order { get; set; }

    public static implicit operator WorkflowDto(Workflow entity)
    {
        return new()
        {
            Id = entity.Id,
            CreatedAt = entity.CreatedAt,
            Name = entity.Name,
            Order = entity.Order
        };
    }

    public static implicit operator Workflow(WorkflowDto dto)
    {
        return Workflow.Create(
            dto.Name,
            dto.Order,
            dto.Id
        );
    }
}
