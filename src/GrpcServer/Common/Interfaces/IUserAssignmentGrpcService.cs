namespace GrpcServer.Common.Interfaces;

public interface IUserAssignmentGrpcService : IService<IUserAssignmentGrpcService>
{
    UnaryResult<OperationResult> CreateUserAssignment(CreateUserAssignmentCommand command);
    
    UnaryResult<GetUserAssignmentByIdQueryViewModel> GetUserAssignmentById(GetUserAssignmentByIdQuery query);

    UnaryResult<ListUserAssignmentQueryViewModel> ListUserAssignment(ListUserAssignmentQuery query);    
    
    UnaryResult<OperationResult> RemoveUserAssignment(RemoveUserAssignmentCommand command);
    
    UnaryResult<OperationResult> UpdateUserAssignment(UpdateUserAssignmentCommand command);
}