using Cpnucleo.Infra.CrossCutting.Util.Commands.Requests.Workflow;
using Cpnucleo.Infra.CrossCutting.Util.Commands.Responses.Workflow;
using Cpnucleo.Infra.CrossCutting.Util.Queries.Requests.Workflow;
using Cpnucleo.Infra.CrossCutting.Util.Queries.Responses.Workflow;
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
