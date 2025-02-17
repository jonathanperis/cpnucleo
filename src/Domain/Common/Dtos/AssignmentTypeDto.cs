namespace Domain.Common.Dtos;

public sealed record AssignmentTypeDto : BaseDto
{
    public string? Name { get; set; }

    public static implicit operator AssignmentTypeDto(AssignmentType entity)
    {
        return new AssignmentTypeDto
        {
            Id = entity.Id,
            CreatedAt = entity.CreatedAt,
            Name = entity.Name
        };
    }

    public static implicit operator AssignmentType(AssignmentTypeDto dto)
    {
        return AssignmentType.Create(
            dto.Name,
            dto.Id
        );
    }
}
