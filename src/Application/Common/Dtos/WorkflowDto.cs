namespace Application.Common.Dtos;

public sealed record WorkflowDto : BaseDto
{
    public string? Name { get; set; }
    public int Order { get; set; }

    // public static implicit operator WorkflowDto?(Workflow? entity)
    // {
    //     if (entity is null)
    //     {
    //         return null;
    //     }
        
    //     var dto = new WorkflowDto
    //     {
    //         Id = entity.Id,
    //         CreatedAt = entity.CreatedAt,
    //         Name = entity.Name,
    //         Order = entity.Order
    //     };
        
    //     return dto;
    // }

    // public static implicit operator Workflow(WorkflowDto dto)
    // {
    //     return Workflow.Create(
    //         dto.Name,
    //         dto.Order,
    //         dto.Id
    //     );
    // }
}
