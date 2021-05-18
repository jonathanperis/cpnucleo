using Cpnucleo.Domain.Commands.Responses.RecursoTarefa;
using MediatR;
using System;

namespace Cpnucleo.Domain.Commands.Requests.RecursoTarefa
{
    public class RemoveRecursoTarefaComand : IRequest<RemoveRecursoTarefaResponse>
    {
        public Guid Id { get; set; }
    }
}
