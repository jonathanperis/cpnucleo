using Cpnucleo.Infra.CrossCutting.Util.Queries.Responses.Recurso;
using MediatR;
using System;

namespace Cpnucleo.Infra.CrossCutting.Util.Queries.Requests.Recurso
{
    public class GetRecursoQuery : IRequest<GetRecursoResponse>
    {
        public Guid Id { get; set; }
    }
}
