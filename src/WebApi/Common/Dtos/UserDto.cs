namespace WebApi.Common.Dtos;

public sealed record UserDto : BaseDto
{
    public string? Name { get; set; }
    public string? Login { get; set; }
}
