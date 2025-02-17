namespace Application.Common.Dtos;

public sealed record AssignmentImpedimentDto : BaseDto
{
    public string? Description { get; set; }
    public Guid AssignmentId { get; set; }
    public Guid ImpedimentId { get; set; }

    public static implicit operator AssignmentImpedimentDto(AssignmentImpediment entity)
    {
        var dto = new AssignmentImpedimentDto()
        {
            Id = entity.Id,
            CreatedAt = entity.CreatedAt
        };
        dto.Description = entity.Description;
        dto.AssignmentId = entity.AssignmentId;
        dto.ImpedimentId = entity.ImpedimentId;
        return dto;
    }

    public static implicit operator AssignmentImpediment(AssignmentImpedimentDto dto)
    {
        return AssignmentImpediment.Create(
            dto.Description,
            dto.AssignmentId,
            dto.ImpedimentId,
            dto.Id
        );
    }
}
