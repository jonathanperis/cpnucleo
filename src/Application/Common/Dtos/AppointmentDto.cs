namespace Application.Common.Dtos;

public sealed record AppointmentDto : BaseDto
{
    public string? Description { get; set; }
    public DateTime KeepDate { get; set; }
    public byte AmountHours { get; set; }
    public Guid AssignmentId { get; set; }
    public Guid UserId { get; set; }

    // public static implicit operator AppointmentDto?(Appointment? entity)
    // {
    //     if (entity is null)
    //     {
    //         return null;
    //     }

    //     var dto = new AppointmentDto
    //     {
    //         Id = entity.Id,
    //         CreatedAt = entity.CreatedAt,
    //         Description = entity.Description,
    //         KeepDate = entity.KeepDate,
    //         AmountHours = entity.AmountHours,
    //         AssignmentId = entity.AssignmentId,
    //         UserId = entity.UserId
    //     };
        
    //     return dto;
    // }
    
    // public static implicit operator Appointment(AppointmentDto dto)
    // {
    //     return Appointment.Create(
    //         dto.Description,
    //         dto.KeepDate,
    //         dto.AmountHours,
    //         dto.AssignmentId,
    //         dto.UserId,
    //         dto.Id
    //     );
    // }
}
