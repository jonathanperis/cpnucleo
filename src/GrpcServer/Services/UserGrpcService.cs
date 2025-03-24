namespace GrpcServer.Services;

internal class UserGrpcService(ISender sender) : ServiceBase<IUserGrpcService>, IUserGrpcService
{
    public async UnaryResult<OperationResult> CreateUser(CreateUserCommand command)
    {
        return await sender.Send(command);
    }

    public async UnaryResult<GetUserByIdQueryViewModel> GetUserById(GetUserByIdQuery query)
    {
        return await sender.Send(query);
    }

    public async UnaryResult<ListUserQueryViewModel> ListUser(ListUserQuery query)
    {
        return await sender.Send(query);
    }

    public async UnaryResult<OperationResult> RemoveUser(RemoveUserCommand command)
    {
        return await sender.Send(command);
    }

    public async UnaryResult<OperationResult> UpdateUser(UpdateUserCommand command)
    {
        return await sender.Send(command);
    }
}