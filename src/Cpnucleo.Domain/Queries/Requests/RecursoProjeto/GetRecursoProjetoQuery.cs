using Cpnucleo.Domain.Queries.Responses.RecursoProjeto;
using MediatR;
using System;

namespace Cpnucleo.Domain.Queries.Requests.RecursoProjeto
{
    public class GetRecursoProjetoQuery : IRequest<GetRecursoProjetoResponse>
    {
        public Guid Id { get; set; }
    }
}
