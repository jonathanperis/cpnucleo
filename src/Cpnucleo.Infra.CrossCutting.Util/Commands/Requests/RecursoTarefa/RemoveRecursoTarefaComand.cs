using Cpnucleo.Infra.CrossCutting.Util.Commands.Responses.RecursoTarefa;
using MediatR;
using System;

namespace Cpnucleo.Infra.CrossCutting.Util.Commands.Requests.RecursoTarefa
{
    public class RemoveRecursoTarefaComand : IRequest<RemoveRecursoTarefaResponse>
    {
        public Guid Id { get; set; }
    }
}
