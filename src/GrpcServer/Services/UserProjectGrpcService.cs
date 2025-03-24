namespace GrpcServer.Services;

internal class UserProjectGrpcService(ISender sender) : ServiceBase<IUserProjectGrpcService>, IUserProjectGrpcService
{
    public async UnaryResult<OperationResult> CreateUserProject(CreateUserProjectCommand command)
    {
        return await sender.Send(command);
    }

    public async UnaryResult<GetUserProjectByIdQueryViewModel> GetUserProjectById(GetUserProjectByIdQuery query)
    {
        return await sender.Send(query);
    }

    public async UnaryResult<ListUserProjectQueryViewModel> ListUserProject(ListUserProjectQuery query)
    {
        return await sender.Send(query);
    }

    public async UnaryResult<OperationResult> RemoveUserProject(RemoveUserProjectCommand command)
    {
        return await sender.Send(command);
    }

    public async UnaryResult<OperationResult> UpdateUserProject(UpdateUserProjectCommand command)
    {
        return await sender.Send(command);
    }
}