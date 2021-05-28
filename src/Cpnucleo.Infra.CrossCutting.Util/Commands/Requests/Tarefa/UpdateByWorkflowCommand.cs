using Cpnucleo.Infra.CrossCutting.Util.Commands.Responses.Tarefa;
using Cpnucleo.Infra.CrossCutting.Util.ViewModels;
using MediatR;
using System;

namespace Cpnucleo.Infra.CrossCutting.Util.Commands.Requests.Tarefa
{
    public class UpdateByWorkflowCommand : IRequest<UpdateByWorkflowResponse>
    {
        public Guid IdTarefa { get; set; }
        public WorkflowViewModel Workflow { get; set; }
    }
}
