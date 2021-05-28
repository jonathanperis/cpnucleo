using Cpnucleo.Infra.CrossCutting.Util.Queries.Responses.Apontamento;
using MediatR;
using System;

namespace Cpnucleo.Infra.CrossCutting.Util.Queries.Requests.Apontamento
{
    public class GetByRecursoQuery : IRequest<GetByRecursoResponse>
    {
        public Guid IdRecurso { get; set; }
    }
}
