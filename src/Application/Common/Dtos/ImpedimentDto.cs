namespace Application.Common.Dtos;

public sealed record ImpedimentDto : BaseDto
{
    public string? Name { get; set; }

    public static implicit operator ImpedimentDto?(Impediment? entity)
    {
        if (entity is null)
        {
            return null;
        }
        
        var dto = new ImpedimentDto
        {
            Id = entity.Id,
            CreatedAt = entity.CreatedAt,
            Name = entity.Name
        };
        
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
