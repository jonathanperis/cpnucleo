namespace Application.UseCases.Appointment.ListAppointment;

// Dapper Repository Advanced
public sealed class ListAppointmentQueryHandler(IUnitOfWork unitOfWork) : IRequestHandler<ListAppointmentQuery, ListAppointmentQueryViewModel>
{
    public async ValueTask<ListAppointmentQueryViewModel> Handle(ListAppointmentQuery request, CancellationToken cancellationToken)
    {
        var repository = unitOfWork.GetRepository<Domain.Entities.Appointment>();
        var response = await repository.GetAllAsync(request.Pagination);        
        
        var operationResult = response.Data != null && response.Data.Any() ? OperationResult.Success : OperationResult.NotFound;

        return new ListAppointmentQueryViewModel(operationResult, MapToPaginatedDto(response));
    }
    
    private static PaginatedResult<AppointmentDto?> MapToPaginatedDto(PaginatedResult<Domain.Entities.Appointment?> result)
    {
        return new PaginatedResult<AppointmentDto?>
        {
            Data = result.Data?.Select(x => x?.MapToDto()).ToList(),
            TotalCount = result.TotalCount,
            PageNumber = result.PageNumber,
            PageSize = result.PageSize
        };
    }
}