namespace Domain.Common.Dtos;

public sealed record UserDto : BaseDto
{
    public string? Name { get; set; }
    public string? Login { get; set; }

    public static implicit operator UserDto(User entity)
    {
        return new()
        {
            Id = entity.Id,
            CreatedAt = entity.CreatedAt,
            Name = entity.Name,
            Login = entity.Login
        };
    }

    public static implicit operator User(UserDto dto)
    {
        return User.Create(
            dto.Name,
            dto.Login,
            string.Empty,
            dto.Id
        );
    }
}
