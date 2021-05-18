using Cpnucleo.Domain.Queries.Responses.Tarefa;
using MediatR;
using System;

namespace Cpnucleo.Domain.Queries.Requests.Tarefa
{
    public class GetByRecursoQuery : IRequest<GetByRecursoResponse>
    {
        public Guid IdRecurso { get; set; }
    }
}
