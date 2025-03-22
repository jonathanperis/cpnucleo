namespace Application.UseCases.Appointment.RemoveAppointment;

public sealed class RemoveAppointmentCommandValidator : AbstractValidator<RemoveAppointmentCommand>
{
    public RemoveAppointmentCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Id is required.");
    }
}
