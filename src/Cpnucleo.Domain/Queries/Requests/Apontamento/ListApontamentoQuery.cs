using Cpnucleo.Domain.Queries.Responses.Apontamento;
using MediatR;

namespace Cpnucleo.Domain.Queries.Requests.Apontamento
{
    public class ListApontamentoQuery : IRequest<ListApontamentoResponse>
    {
        public bool GetDependencies { get; set; }
    }
}
