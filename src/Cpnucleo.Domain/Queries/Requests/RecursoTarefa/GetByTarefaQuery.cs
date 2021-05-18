using Cpnucleo.Domain.Queries.Responses.RecursoTarefa;
using MediatR;
using System;

namespace Cpnucleo.Domain.Queries.Requests.RecursoTarefa
{
    public class GetByTarefaQuery : IRequest<GetByTarefaResponse>
    {
        public Guid IdTarefa { get; set; }
    }
}
