using Cpnucleo.Infra.CrossCutting.Util.Queries.Responses.RecursoProjeto;
using MediatR;
using System;

namespace Cpnucleo.Infra.CrossCutting.Util.Queries.Requests.RecursoProjeto
{
    public class GetRecursoProjetoQuery : IRequest<GetRecursoProjetoResponse>
    {
        public Guid Id { get; set; }
    }
}
