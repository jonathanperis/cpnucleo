namespace Application.Common.Dtos;

public abstract record BaseDto
{
    public Guid Id { get; init; }
    public DateTime CreatedAt { get; init; }
}