namespace GrpcServer.Contracts.Common.Dtos;

public sealed record AssignmentImpedimentDto : BaseDto
{
    public string? Description { get; set; }
    public Guid AssignmentId { get; set; }
    public Guid ImpedimentId { get; set; }

    // public static implicit operator AssignmentImpedimentDto?(AssignmentImpediment? entity)
    // {
    //     if (entity is null)
    //     {
    //         return null;
    //     }

    //     var dto = new AssignmentImpedimentDto
    //     {
    //         Id = entity.Id,
    //         CreatedAt = entity.CreatedAt,
    //         Description = entity.Description,
    //         AssignmentId = entity.AssignmentId,
    //         ImpedimentId = entity.ImpedimentId
    //     };

    //     return dto;
    // }

    // public static implicit operator AssignmentImpediment(AssignmentImpedimentDto dto)
    // {
    //     return AssignmentImpediment.Create(
    //         dto.Description,
    //         dto.AssignmentId,
    //         dto.ImpedimentId,
    //         dto.Id
    //     );
    // }
}
