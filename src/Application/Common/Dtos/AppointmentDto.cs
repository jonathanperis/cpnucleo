namespace Application.Common.Dtos;

public sealed record AppointmentDto : BaseDto
{
    public string? Description { get; set; }
    public DateTime KeepDate { get; set; }
    public byte AmountHours { get; set; }
    public Guid AssignmentId { get; set; }
    public Guid UserId { get; set; }

    public static implicit operator AppointmentDto(Appointment entity)
    {
        var dto = new AppointmentDto()
        {
            Id = entity.Id,
            CreatedAt = entity.CreatedAt
        };
        dto.Description = entity.Description;
        dto.KeepDate = entity.KeepDate;
        dto.AmountHours = entity.AmountHours;
        dto.AssignmentId = entity.AssignmentId;
        dto.UserId = entity.UserId;
        return dto;
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
