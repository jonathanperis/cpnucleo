using Cpnucleo.Infra.CrossCutting.Util.Commands.Responses.Workflow;
using MediatR;
using System;

namespace Cpnucleo.Infra.CrossCutting.Util.Commands.Requests.Workflow
{
    public class RemoveWorkflowComand : IRequest<RemoveWorkflowResponse>
    {
        public Guid Id { get; set; }
    }
}
