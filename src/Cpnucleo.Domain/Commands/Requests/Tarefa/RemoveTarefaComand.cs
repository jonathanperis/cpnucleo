using Cpnucleo.Domain.Commands.Responses.Tarefa;
using MediatR;
using System;

namespace Cpnucleo.Domain.Commands.Requests.Tarefa
{
    public class RemoveTarefaComand : IRequest<RemoveTarefaResponse>
    {
        public Guid Id { get; set; }
    }
}
