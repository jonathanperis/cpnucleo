namespace Application.UseCases.Appointment.GetAppointmentById;

// Dapper Repository Advanced
public sealed class GetAppointmentByIdQueryHandler(IUnitOfWork unitOfWork) : IRequestHandler<GetAppointmentByIdQuery, GetAppointmentByIdQueryViewModel>
{
    public async ValueTask<GetAppointmentByIdQueryViewModel> Handle(GetAppointmentByIdQuery request, CancellationToken cancellationToken)
    {
        var repository = unitOfWork.GetRepository<Domain.Entities.Appointment>();
        var response = await repository.GetByIdAsync(request.Id);        
        
        var operationResult = response is not null ? OperationResult.Success : OperationResult.NotFound;

        return new GetAppointmentByIdQueryViewModel(operationResult, response?.MapToDto());
    }
}