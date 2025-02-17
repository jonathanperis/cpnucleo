namespace Application.Common.Dtos;

public sealed record UserDto : BaseDto
{
    public string? Name { get; set; }
    public string? Login { get; set; }

    public static implicit operator UserDto(User entity)
    {
        var dto = new UserDto()
        {
            Id = entity.Id,
            CreatedAt = entity.CreatedAt
        };
        dto.Name = entity.Name;
        dto.Login = entity.Login;
        return dto;
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
