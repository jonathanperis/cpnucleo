using Cpnucleo.Domain.Commands.Responses.Workflow;
using MediatR;

namespace Cpnucleo.Domain.Commands.Requests.Workflow
{
    public class UpdateWorkflowComand : IRequest<UpdateWorkflowResponse>
    {
        public Domain.Entities.Workflow Workflow { get; set; }
    }
}
