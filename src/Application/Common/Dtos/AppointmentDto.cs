namespace Application.Common.Dtos;

public sealed record AppointmentDto(string? Description, DateTime KeepDate, byte AmountHours, Ulid AssignmentId, Ulid UserId) : BaseDto;
