namespace Domain.Common.Dtos;

public sealed record AssignmentImpedimentDto : BaseDto
{
    public string? Description { get; set; }
    public Ulid AssignmentId { get; set; }
    public Ulid ImpedimentId { get; set; }

    public static implicit operator AssignmentImpedimentDto(AssignmentImpediment entity)
    {
        return new()
        {
            Id = entity.Id,
            CreatedAt = entity.CreatedAt,
            Description = entity.Description,
            AssignmentId = entity.AssignmentId,
            ImpedimentId = entity.ImpedimentId
        };
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
