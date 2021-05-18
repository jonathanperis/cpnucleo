using Cpnucleo.Domain.Queries.Responses.Projeto;
using MediatR;
using System;

namespace Cpnucleo.Domain.Queries.Requests.Projeto
{
    public class GetProjetoQuery : IRequest<GetProjetoResponse>
    {
        public Guid Id { get; set; }
    }
}
