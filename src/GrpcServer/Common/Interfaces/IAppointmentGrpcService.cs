namespace GrpcServer.Common.Interfaces;

public interface IAppointmentGrpcService : IService<IAppointmentGrpcService>
{
    UnaryResult<OperationResult> CreateAppointment(CreateAppointmentCommand command);
    
    UnaryResult<GetAppointmentByIdQueryViewModel> GetAppointmentById(GetAppointmentByIdQuery query);

    UnaryResult<ListAppointmentQueryViewModel> ListAppointment(ListAppointmentQuery query);    
    
    UnaryResult<OperationResult> RemoveAppointment(RemoveAppointmentCommand command);
    
    UnaryResult<OperationResult> UpdateAppointment(UpdateAppointmentCommand command);
}