namespace Cpnucleo.Infra.CrossCutting.Shared.Common.Interfaces;

public interface IWorkflowGrpcService : IService<IWorkflowGrpcService>
{
    UnaryResult<OperationResult> CreateWorkflow(CreateWorkflowCommand command);

    UnaryResult<OperationResult> UpdateWorkflow(UpdateWorkflowCommand command);

    UnaryResult<GetWorkflowViewModel> GetWorkflow(GetWorkflowQuery query);

    UnaryResult<ListWorkflowViewModel> ListWorkflow(ListWorkflowQuery query);

    UnaryResult<OperationResult> RemoveWorkflow(RemoveWorkflowCommand command);
}