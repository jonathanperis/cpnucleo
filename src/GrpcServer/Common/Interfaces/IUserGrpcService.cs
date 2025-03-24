namespace GrpcServer.Common.Interfaces;

public interface IUserGrpcService : IService<IUserGrpcService>
{
    UnaryResult<OperationResult> CreateUser(CreateUserCommand command);
    
    UnaryResult<GetUserByIdQueryViewModel> GetUserById(GetUserByIdQuery query);

    UnaryResult<ListUserQueryViewModel> ListUser(ListUserQuery query);    
    
    UnaryResult<OperationResult> RemoveUser(RemoveUserCommand command);
    
    UnaryResult<OperationResult> UpdateUser(UpdateUserCommand command);
}