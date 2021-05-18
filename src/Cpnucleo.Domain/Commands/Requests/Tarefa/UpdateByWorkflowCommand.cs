using Cpnucleo.Domain.Commands.Responses.Tarefa;
using MediatR;
using System;

namespace Cpnucleo.Domain.Commands.Requests.Tarefa
{
    public class UpdateByWorkflowCommand : IRequest<UpdateByWorkflowResponse>
    {
        public Guid IdTarefa { get; set; }
        public Entities.Workflow Workflow { get; set; }
    }
}
