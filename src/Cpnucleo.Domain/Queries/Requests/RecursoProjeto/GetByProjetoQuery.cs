using Cpnucleo.Domain.Queries.Responses.RecursoProjeto;
using MediatR;
using System;

namespace Cpnucleo.Domain.Queries.Requests.RecursoProjeto
{
    public class GetByProjetoQuery : IRequest<GetByProjetoResponse>
    {
        public Guid IdProjeto { get; set; }
    }
}
