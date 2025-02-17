namespace Application.Common.Dtos;

public sealed record AssignmentTypeDto : BaseDto
{
    public string? Name { get; set; }

    public static implicit operator AssignmentTypeDto(AssignmentType entity)
    {
        var dto = new AssignmentTypeDto
        {
            Id = entity.Id,
            CreatedAt = entity.CreatedAt
        };
        dto.Name = entity.Name;
        return dto;
    }

    public static implicit operator AssignmentType(AssignmentTypeDto dto)
    {
        return AssignmentType.Create(
            dto.Name,
            dto.Id
        );
    }
}
