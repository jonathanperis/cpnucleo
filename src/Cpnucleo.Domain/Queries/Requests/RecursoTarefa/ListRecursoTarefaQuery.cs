using Cpnucleo.Domain.Queries.Responses.RecursoTarefa;
using MediatR;

namespace Cpnucleo.Domain.Queries.Requests.RecursoTarefa
{
    public class ListRecursoTarefaQuery : IRequest<ListRecursoTarefaResponse>
    {
        public bool GetDependencies { get; set; }
    }
}
