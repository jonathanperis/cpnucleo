namespace GrpcServer.Common.Interfaces;

public interface IAssignmentGrpcService : IService<IAssignmentGrpcService>
{
    UnaryResult<OperationResult> CreateAssignment(CreateAssignmentCommand command);
    
    UnaryResult<GetAssignmentByIdQueryViewModel> GetAssignmentById(GetAssignmentByIdQuery query);

    UnaryResult<ListAssignmentQueryViewModel> ListAssignment(ListAssignmentQuery query);    
    
    UnaryResult<OperationResult> RemoveAssignment(RemoveAssignmentCommand command);
    
    UnaryResult<OperationResult> UpdateAssignment(UpdateAssignmentCommand command);
}