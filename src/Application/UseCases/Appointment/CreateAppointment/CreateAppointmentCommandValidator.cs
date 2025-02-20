namespace Application.UseCases.Appointment.CreateAppointment;

public sealed class CreateAppointmentCommandValidator : AbstractValidator<CreateAppointmentCommand>
{
    public CreateAppointmentCommandValidator()
    {
        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("Description is required.");

        RuleFor(x => x.KeepDate)
            .NotEmpty().WithMessage("KeepDate is required.");

        RuleFor(x => x.AmountHours)
            .GreaterThan((int)0).WithMessage("AmountHours must be greater than 0.");

        RuleFor(x => x.AssignmentId)
            .NotEmpty().WithMessage("AssignmentId is required.");

        RuleFor(x => x.UserId)
            .NotEmpty().WithMessage("UserId is required.");
    }
}
