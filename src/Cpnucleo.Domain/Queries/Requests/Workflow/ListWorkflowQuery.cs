using Cpnucleo.Domain.Queries.Responses.Workflow;
using MediatR;

namespace Cpnucleo.Domain.Queries.Requests.Workflow
{
    public class ListWorkflowQuery : IRequest<ListWorkflowResponse>
    {
        public bool GetDependencies { get; set; }
    }
}
