namespace GrpcServer.Common.Interfaces;

public interface IAssignmentTypeGrpcService : IService<IAssignmentTypeGrpcService>
{
    UnaryResult<OperationResult> CreateAssignmentType(CreateAssignmentTypeCommand command);
    
    UnaryResult<GetAssignmentTypeByIdQueryViewModel> GetAssignmentTypeById(GetAssignmentTypeByIdQuery query);

    UnaryResult<ListAssignmentTypeQueryViewModel> ListAssignmentType(ListAssignmentTypeQuery query);    
    
    UnaryResult<OperationResult> RemoveAssignmentType(RemoveAssignmentTypeCommand command);
    
    UnaryResult<OperationResult> UpdateAssignmentType(UpdateAssignmentTypeCommand command);
}