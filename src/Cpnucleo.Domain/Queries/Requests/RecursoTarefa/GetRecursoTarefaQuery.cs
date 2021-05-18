using Cpnucleo.Domain.Queries.Responses.RecursoTarefa;
using MediatR;
using System;

namespace Cpnucleo.Domain.Queries.Requests.RecursoTarefa
{
    public class GetRecursoTarefaQuery : IRequest<GetRecursoTarefaResponse>
    {
        public Guid Id { get; set; }
    }
}
