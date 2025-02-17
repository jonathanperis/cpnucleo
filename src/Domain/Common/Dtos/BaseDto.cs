namespace Domain.Common.Dtos;

public abstract record BaseDto
{
    public required Ulid Id { get; init; }
    public required DateTime CreatedAt { get; init; }
}