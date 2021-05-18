using Cpnucleo.Domain.Queries.Responses.Recurso;
using MediatR;
using System;

namespace Cpnucleo.Domain.Queries.Requests.Recurso
{
    public class GetRecursoQuery : IRequest<GetRecursoResponse>
    {
        public Guid Id { get; set; }
    }
}
