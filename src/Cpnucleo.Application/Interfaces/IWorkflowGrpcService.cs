using Cpnucleo.Application.Commands.Workflow.CreateWorkflow;
using Cpnucleo.Application.Commands.Workflow.RemoveWorkflow;
using Cpnucleo.Application.Commands.Workflow.UpdateWorkflow;
using Cpnucleo.Application.Queries.Workflow.GetWorkflow;
using Cpnucleo.Application.Queries.Workflow.ListWorkflow;

namespace Cpnucleo.Application.Interfaces;

public interface IWorkflowGrpcService : IService<IWorkflowGrpcService>
{
    UnaryResult<OperationResult> AddAsync(CreateWorkflowCommand command);

    UnaryResult<OperationResult> UpdateAsync(UpdateWorkflowCommand command);

    UnaryResult<GetWorkflowViewModel> GetAsync(GetWorkflowQuery query);

    UnaryResult<ListWorkflowViewModel> AllAsync(ListWorkflowQuery query);

    UnaryResult<OperationResult> RemoveAsync(RemoveWorkflowCommand command);
}