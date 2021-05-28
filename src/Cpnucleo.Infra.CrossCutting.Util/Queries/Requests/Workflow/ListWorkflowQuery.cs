using Cpnucleo.Infra.CrossCutting.Util.Queries.Responses.Workflow;
using MediatR;

namespace Cpnucleo.Infra.CrossCutting.Util.Queries.Requests.Workflow
{
    public class ListWorkflowQuery : IRequest<ListWorkflowResponse>
    {
        public bool GetDependencies { get; set; }
    }
}
