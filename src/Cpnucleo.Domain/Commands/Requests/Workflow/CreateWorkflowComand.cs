using Cpnucleo.Domain.Commands.Responses.Workflow;
using MediatR;

namespace Cpnucleo.Domain.Commands.Requests.Workflow
{
    public class CreateWorkflowComand : IRequest<CreateWorkflowResponse>
    {
        public Domain.Entities.Workflow Workflow { get; set; }
    }
}
