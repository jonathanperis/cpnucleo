namespace Cpnucleo.Shared.Common.Dtos;

[MessagePackObject(true)]
public abstract record BaseDto
{
    public Guid Id { get; set; }

    public DateTime DataInclusao { get; set; }
}