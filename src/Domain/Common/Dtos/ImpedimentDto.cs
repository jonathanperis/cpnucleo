namespace Domain.Common.Dtos;

public sealed record ImpedimentDto : BaseDto
{
    public string? Name { get; set; }

    public static implicit operator ImpedimentDto(Impediment entity)
    {
        return new ImpedimentDto
        {
            Id = entity.Id,
            CreatedAt = entity.CreatedAt,
            Name = entity.Name
        };
    }

    public static implicit operator Impediment(ImpedimentDto dto)
    {
        return Impediment.Create(
                dto.Name,
                dto.Id
            );
    }
}
