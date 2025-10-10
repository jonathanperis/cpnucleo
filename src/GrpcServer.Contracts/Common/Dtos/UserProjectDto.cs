namespace GrpcServer.Contracts.Common.Dtos;

public sealed record UserProjectDto : BaseDto
{
    public Guid UserId { get; set; }
    public Guid ProjectId { get; set; }

    // public static implicit operator UserProjectDto?(UserProject? entity)
    // {
    //     if (entity is null)
    //     {
    //         return null;
    //     }

    //     var dto = new UserProjectDto
    //     {
    //         Id = entity.Id,
    //         CreatedAt = entity.CreatedAt,
    //         UserId = entity.UserId,
    //         ProjectId = entity.ProjectId
    //     };

    //     return dto;
    // }

    // public static implicit operator UserProject(UserProjectDto dto)
    // {
    //     return UserProject.Create(
    //         dto.UserId,
    //         dto.ProjectId,
    //         dto.Id
    //     );
    // }
}
