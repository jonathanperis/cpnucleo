using Cpnucleo.Domain.Queries.Responses.Recurso;
using MediatR;

namespace Cpnucleo.Domain.Queries.Requests.Recurso
{
    public class ListRecursoQuery : IRequest<ListRecursoResponse>
    {
        public bool GetDependencies { get; set; }
    }
}
