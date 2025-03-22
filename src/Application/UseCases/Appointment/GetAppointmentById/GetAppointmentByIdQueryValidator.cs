namespace Application.UseCases.Appointment.GetAppointmentById;

public sealed class GetAppointmentByIdQueryValidator : AbstractValidator<GetAppointmentByIdQuery>
{
    public GetAppointmentByIdQueryValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Id is required.");
    }
}
