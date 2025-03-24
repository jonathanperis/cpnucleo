namespace GrpcServer.Common.Interfaces;

public interface IAssignmentImpedimentGrpcService : IService<IAssignmentImpedimentGrpcService>
{
    UnaryResult<OperationResult> CreateAssignmentImpediment(CreateAssignmentImpedimentCommand command);
    
    UnaryResult<GetAssignmentImpedimentByIdQueryViewModel> GetAssignmentImpedimentById(GetAssignmentImpedimentByIdQuery query);

    UnaryResult<ListAssignmentImpedimentQueryViewModel> ListAssignmentImpediment(ListAssignmentImpedimentQuery query);    
    
    UnaryResult<OperationResult> RemoveAssignmentImpediment(RemoveAssignmentImpedimentCommand command);
    
    UnaryResult<OperationResult> UpdateAssignmentImpediment(UpdateAssignmentImpedimentCommand command);
}