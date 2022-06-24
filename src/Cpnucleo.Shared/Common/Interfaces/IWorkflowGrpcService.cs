using Cpnucleo.Shared.Commands.Workflow;
using Cpnucleo.Shared.Common.Models;
using Cpnucleo.Shared.Queries.Workflow;

namespace Cpnucleo.Shared.Common.Interfaces;

public interface IWorkflowGrpcService : IService<IWorkflowGrpcService>
{
    UnaryResult<OperationResult> CreateWorkflow(CreateWorkflowCommand command);

    UnaryResult<OperationResult> UpdateWorkflow(UpdateWorkflowCommand command);

    UnaryResult<GetWorkflowViewModel> GetWorkflow(GetWorkflowQuery query);

    UnaryResult<ListWorkflowViewModel> ListWorkflow(ListWorkflowQuery query);

    UnaryResult<OperationResult> RemoveWorkflow(RemoveWorkflowCommand command);
}