using Cpnucleo.Infra.CrossCutting.Util.Commands.Workflow.CreateWorkflow;
using Cpnucleo.Infra.CrossCutting.Util.Commands.Workflow.RemoveWorkflow;
using Cpnucleo.Infra.CrossCutting.Util.Commands.Workflow.UpdateWorkflow;
using Cpnucleo.Infra.CrossCutting.Util.Queries.Workflow.GetWorkflow;
using Cpnucleo.Infra.CrossCutting.Util.Queries.Workflow.ListWorkflow;
using MagicOnion;

namespace Cpnucleo.Infra.CrossCutting.Util.Interfaces
{
    public interface IWorkflowGrpcService : IService<IWorkflowGrpcService>
    {
        UnaryResult<CreateWorkflowResponse> AddAsync(CreateWorkflowCommand command);

        UnaryResult<UpdateWorkflowResponse> UpdateAsync(UpdateWorkflowCommand command);

        UnaryResult<GetWorkflowResponse> GetAsync(GetWorkflowQuery query);

        UnaryResult<ListWorkflowResponse> AllAsync(ListWorkflowQuery query);

        UnaryResult<RemoveWorkflowResponse> RemoveAsync(RemoveWorkflowCommand command);
    }
}
