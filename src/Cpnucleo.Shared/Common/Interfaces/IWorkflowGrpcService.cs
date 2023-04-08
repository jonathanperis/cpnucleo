using Cpnucleo.Shared.Commands.CreateWorkflow;
using Cpnucleo.Shared.Commands.RemoveWorkflow;
using Cpnucleo.Shared.Commands.UpdateWorkflow;
using Cpnucleo.Shared.Queries.GetWorkflow;
using Cpnucleo.Shared.Queries.ListWorkflow;

namespace Cpnucleo.Shared.Common.Interfaces;

public interface IWorkflowGrpcService : IService<IWorkflowGrpcService>
{
    UnaryResult<OperationResult> CreateWorkflow(CreateWorkflowCommand command);

    UnaryResult<OperationResult> UpdateWorkflow(UpdateWorkflowCommand command);

    UnaryResult<GetWorkflowViewModel> GetWorkflow(GetWorkflowQuery query);

    UnaryResult<ListWorkflowViewModel> ListWorkflow(ListWorkflowQuery query);

    UnaryResult<OperationResult> RemoveWorkflow(RemoveWorkflowCommand command);
}