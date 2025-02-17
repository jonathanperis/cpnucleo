namespace Application.Common.Dtos;

public sealed record ImpedimentDto : BaseDto
{
    public string? Name { get; set; }

    public static implicit operator ImpedimentDto(Impediment entity)
    {
        var dto = new ImpedimentDto
        {
            Id = entity.Id,
            CreatedAt = entity.CreatedAt
        };
        dto.Name = entity.Name;
        return dto;
    }

    public static implicit operator Impediment(ImpedimentDto dto)
    {
        return Impediment.Create(
                dto.Name,
                dto.Id
            );
    }
}
