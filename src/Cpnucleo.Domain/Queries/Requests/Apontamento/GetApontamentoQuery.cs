using Cpnucleo.Domain.Queries.Responses.Apontamento;
using MediatR;
using System;

namespace Cpnucleo.Domain.Queries.Requests.Apontamento
{
    public class GetApontamentoQuery : IRequest<GetApontamentoResponse>
    {
        public Guid Id { get; set; }
    }
}
