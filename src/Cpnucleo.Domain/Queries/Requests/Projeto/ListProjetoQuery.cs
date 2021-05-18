using Cpnucleo.Domain.Queries.Responses.Projeto;
using MediatR;

namespace Cpnucleo.Domain.Queries.Requests.Projeto
{
    public class ListProjetoQuery : IRequest<ListProjetoResponse>
    {
        public bool GetDependencies { get; set; }
    }
}
