using Cpnucleo.Application.Commands.Workflow.CreateWorkflow;
using Cpnucleo.Application.Commands.Workflow.RemoveWorkflow;
using Cpnucleo.Application.Commands.Workflow.UpdateWorkflow;
using Cpnucleo.Application.Queries.Workflow.GetWorkflow;
using Cpnucleo.Application.Queries.Workflow.ListWorkflow;
using MagicOnion;

namespace Cpnucleo.Application.Interfaces;

public interface IWorkflowGrpcService : IService<IWorkflowGrpcService>
{
    UnaryResult<OperationResult> CreateWorkflow(CreateWorkflowCommand command);

    UnaryResult<OperationResult> UpdateWorkflow(UpdateWorkflowCommand command);

    UnaryResult<GetWorkflowViewModel> GetWorkflow(GetWorkflowQuery query);

    UnaryResult<ListWorkflowViewModel> ListWorkflow(ListWorkflowQuery query);

    UnaryResult<OperationResult> RemoveWorkflow(RemoveWorkflowCommand command);
}