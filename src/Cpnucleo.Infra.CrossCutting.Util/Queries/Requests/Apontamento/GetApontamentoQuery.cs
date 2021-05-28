using Cpnucleo.Infra.CrossCutting.Util.Queries.Responses.Apontamento;
using MediatR;
using System;

namespace Cpnucleo.Infra.CrossCutting.Util.Queries.Requests.Apontamento
{
    public class GetApontamentoQuery : IRequest<GetApontamentoResponse>
    {
        public Guid Id { get; set; }
    }
}
