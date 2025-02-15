namespace Domain.Common.Dtos;

//[MessagePackObject(true)]
// public abstract record BaseDto(Ulid Id, DateTime CreatedAt);

public abstract record BaseDto
{
    public required Ulid Id { get; set; }
    public required DateTime CreatedAt { get; set; }
}