namespace Application.Common.Dtos;

public abstract record BaseDto
{
    protected Guid Id { get; init; }
    protected DateTime CreatedAt { get; init; }
}