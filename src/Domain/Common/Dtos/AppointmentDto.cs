namespace Domain.Common.Dtos;

public sealed record AppointmentDto : BaseDto
{
    public string? Description { get; set; }
    public DateTime KeepDate { get; set; }
    public byte AmountHours { get; set; }
    public Ulid AssignmentId { get; set; }
    public Ulid UserId { get; set; }

    public static implicit operator AppointmentDto(Appointment entity)
    {
        return new()
        {
            Id = entity.Id,
            CreatedAt = entity.CreatedAt,
            Description = entity.Description,
            KeepDate = entity.KeepDate,
            AmountHours = entity.AmountHours,
            AssignmentId = entity.AssignmentId,
            UserId = entity.UserId
        };
    }

    public static implicit operator Appointment(AppointmentDto dto)
    {
        return Appointment.Create(
            dto.Description,
            dto.KeepDate,
            dto.AmountHours,
            dto.AssignmentId,
            dto.UserId,
            dto.Id
        );
    }
}
