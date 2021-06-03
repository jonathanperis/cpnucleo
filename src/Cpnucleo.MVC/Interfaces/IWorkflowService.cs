using Cpnucleo.Infra.CrossCutting.Util.Commands.Workflow.CreateWorkflow;
using Cpnucleo.Infra.CrossCutting.Util.Commands.Workflow.RemoveWorkflow;
using Cpnucleo.Infra.CrossCutting.Util.Commands.Workflow.UpdateWorkflow;
using Cpnucleo.Infra.CrossCutting.Util.Queries.Workflow.GetWorkflow;
using Cpnucleo.Infra.CrossCutting.Util.Queries.Workflow.ListWorkflow;
using System.Threading.Tasks;

namespace Cpnucleo.MVC.Interfaces
{
    public interface IWorkflowService
    {
        Task<CreateWorkflowResponse> AddAsync(string token, CreateWorkflowCommand command);

        Task<UpdateWorkflowResponse> UpdateAsync(string token, UpdateWorkflowCommand command);

        Task<GetWorkflowResponse> GetAsync(string token, GetWorkflowQuery query);

        Task<ListWorkflowResponse> AllAsync(string token, ListWorkflowQuery query);

        Task<RemoveWorkflowResponse> RemoveAsync(string token, RemoveWorkflowCommand command);
    }
}
