using Cpnucleo.Infra.CrossCutting.Util.Commands.Responses.RecursoTarefa;
using MediatR;
using System;

namespace Cpnucleo.Infra.CrossCutting.Util.Commands.Requests.RecursoTarefa
{
    public class RemoveRecursoTarefaCommand : IRequest<RemoveRecursoTarefaResponse>
    {
        public Guid Id { get; set; }
    }
}
