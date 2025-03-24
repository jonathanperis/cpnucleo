namespace GrpcServer.Services;

internal class UserAssignmentGrpcService(ISender sender) : ServiceBase<IUserAssignmentGrpcService>, IUserAssignmentGrpcService
{
    public async UnaryResult<OperationResult> CreateUserAssignment(CreateUserAssignmentCommand command)
    {
        return await sender.Send(command);
    }

    public async UnaryResult<GetUserAssignmentByIdQueryViewModel> GetUserAssignmentById(GetUserAssignmentByIdQuery query)
    {
        return await sender.Send(query);
    }

    public async UnaryResult<ListUserAssignmentQueryViewModel> ListUserAssignment(ListUserAssignmentQuery query)
    {
        return await sender.Send(query);
    }

    public async UnaryResult<OperationResult> RemoveUserAssignment(RemoveUserAssignmentCommand command)
    {
        return await sender.Send(command);
    }

    public async UnaryResult<OperationResult> UpdateUserAssignment(UpdateUserAssignmentCommand command)
    {
        return await sender.Send(command);
    }
}