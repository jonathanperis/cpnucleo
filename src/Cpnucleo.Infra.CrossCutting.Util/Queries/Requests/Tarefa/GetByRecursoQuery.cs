using Cpnucleo.Infra.CrossCutting.Util.Queries.Responses.Tarefa;
using MediatR;
using System;

namespace Cpnucleo.Infra.CrossCutting.Util.Queries.Requests.Tarefa
{
    public class GetByRecursoQuery : IRequest<GetByRecursoResponse>
    {
        public Guid IdRecurso { get; set; }
    }
}
