using Cpnucleo.Infra.CrossCutting.Util.Queries.Responses.Projeto;
using MediatR;

namespace Cpnucleo.Infra.CrossCutting.Util.Queries.Requests.Projeto
{
    public class ListProjetoQuery : IRequest<ListProjetoResponse>
    {
        public bool GetDependencies { get; set; }
    }
}
