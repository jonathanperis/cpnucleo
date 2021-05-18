using Cpnucleo.Domain.Queries.Responses.Tarefa;
using MediatR;
using System;

namespace Cpnucleo.Domain.Queries.Requests.Tarefa
{
    public class GetTarefaQuery : IRequest<GetTarefaResponse>
    {
        public Guid Id { get; set; }
    }
}
