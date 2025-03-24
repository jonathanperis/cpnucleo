namespace GrpcServer.Services;

internal class AppointmentGrpcService(ISender sender) : ServiceBase<IAppointmentGrpcService>, IAppointmentGrpcService
{
    public async UnaryResult<OperationResult> CreateAppointment(CreateAppointmentCommand command)
    {
        return await sender.Send(command);
    }

    public async UnaryResult<GetAppointmentByIdQueryViewModel> GetAppointmentById(GetAppointmentByIdQuery query)
    {
        return await sender.Send(query);
    }

    public async UnaryResult<ListAppointmentQueryViewModel> ListAppointment(ListAppointmentQuery query)
    {
        return await sender.Send(query);
    }

    public async UnaryResult<OperationResult> RemoveAppointment(RemoveAppointmentCommand command)
    {
        return await sender.Send(command);
    }

    public async UnaryResult<OperationResult> UpdateAppointment(UpdateAppointmentCommand command)
    {
        return await sender.Send(command);
    }
}