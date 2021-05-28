using Cpnucleo.Infra.CrossCutting.Util.Queries.Responses.Apontamento;
using MediatR;

namespace Cpnucleo.Infra.CrossCutting.Util.Queries.Requests.Apontamento
{
    public class ListApontamentoQuery : IRequest<ListApontamentoResponse>
    {
        public bool GetDependencies { get; set; }
    }
}
