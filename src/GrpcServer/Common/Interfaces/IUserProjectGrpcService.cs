namespace GrpcServer.Common.Interfaces;

public interface IUserProjectGrpcService : IService<IUserProjectGrpcService>
{
    UnaryResult<OperationResult> CreateUserProject(CreateUserProjectCommand command);
    
    UnaryResult<GetUserProjectByIdQueryViewModel> GetUserProjectById(GetUserProjectByIdQuery query);

    UnaryResult<ListUserProjectQueryViewModel> ListUserProject(ListUserProjectQuery query);    
    
    UnaryResult<OperationResult> RemoveUserProject(RemoveUserProjectCommand command);
    
    UnaryResult<OperationResult> UpdateUserProject(UpdateUserProjectCommand command);
}