using Cpnucleo.Infra.CrossCutting.Util.Commands.Responses.Tarefa;
using MediatR;
using System;

namespace Cpnucleo.Infra.CrossCutting.Util.Commands.Requests.Tarefa
{
    public class RemoveTarefaCommand : IRequest<RemoveTarefaResponse>
    {
        public Guid Id { get; set; }
    }
}
