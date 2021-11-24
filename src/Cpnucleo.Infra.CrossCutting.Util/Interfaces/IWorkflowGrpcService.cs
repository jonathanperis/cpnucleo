using Cpnucleo.Infra.CrossCutting.Util.Commands.Workflow;
using Cpnucleo.Infra.CrossCutting.Util.Queries.Workflow;

namespace Cpnucleo.Infra.CrossCutting.Util.Interfaces;

public interface IWorkflowGrpcService : IService<IWorkflowGrpcService>
{
    UnaryResult<OperationResult> AddAsync(CreateWorkflowCommand command);

    UnaryResult<OperationResult> UpdateAsync(UpdateWorkflowCommand command);

    UnaryResult<WorkflowViewModel> GetAsync(GetWorkflowQuery query);

    UnaryResult<IEnumerable<WorkflowViewModel>> AllAsync(ListWorkflowQuery query);

    UnaryResult<OperationResult> RemoveAsync(RemoveWorkflowCommand command);
}