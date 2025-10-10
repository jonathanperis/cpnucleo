namespace GrpcServer.Contracts.Common.Dtos;

public sealed record UserDto : BaseDto
{
    public string? Name { get; set; }
    public string? Login { get; set; }

    // public static implicit operator UserDto?(User? entity)
    // {
    //     if (entity is null)
    //     {
    //         return null;
    //     }

    //     var dto = new UserDto
    //     {
    //         Id = entity.Id,
    //         CreatedAt = entity.CreatedAt,
    //         Name = entity.Name,
    //         Login = entity.Login
    //     };

    //     return dto;
    // }

    // public static implicit operator User(UserDto dto)
    // {
    //     return User.Create(
    //         dto.Name,
    //         dto.Login,
    //         string.Empty,
    //         dto.Id
    //     );
    // }
}
