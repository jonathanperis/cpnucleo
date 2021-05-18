using Cpnucleo.Domain.Commands.Responses.Workflow;
using MediatR;
using System;

namespace Cpnucleo.Domain.Commands.Requests.Workflow
{
    public class RemoveWorkflowComand : IRequest<RemoveWorkflowResponse>
    {
        public Guid Id { get; set; }
    }
}
