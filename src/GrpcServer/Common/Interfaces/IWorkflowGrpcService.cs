namespace GrpcServer.Common.Interfaces;

public interface IWorkflowGrpcService : IService<IWorkflowGrpcService>
{
    UnaryResult<OperationResult> CreateWorkflow(CreateWorkflowCommand command);
    
    UnaryResult<GetWorkflowByIdQueryViewModel> GetWorkflowById(GetWorkflowByIdQuery query);

    UnaryResult<ListWorkflowQueryViewModel> ListWorkflow(ListWorkflowQuery query);    
    
    UnaryResult<OperationResult> RemoveWorkflow(RemoveWorkflowCommand command);
    
    UnaryResult<OperationResult> UpdateWorkflow(UpdateWorkflowCommand command);
}