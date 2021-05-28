using Cpnucleo.Infra.CrossCutting.Util.Queries.Responses.RecursoTarefa;
using MediatR;
using System;

namespace Cpnucleo.Infra.CrossCutting.Util.Queries.Requests.RecursoTarefa
{
    public class GetByTarefaQuery : IRequest<GetByTarefaResponse>
    {
        public Guid IdTarefa { get; set; }
    }
}
