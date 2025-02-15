namespace Domain.Common.Dtos;

public sealed record UserDto(string? Name, string? Login) : BaseDto;
