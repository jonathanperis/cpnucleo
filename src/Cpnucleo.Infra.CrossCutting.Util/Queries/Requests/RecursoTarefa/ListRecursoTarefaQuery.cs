using Cpnucleo.Infra.CrossCutting.Util.Queries.Responses.RecursoTarefa;
using MediatR;

namespace Cpnucleo.Infra.CrossCutting.Util.Queries.Requests.RecursoTarefa
{
    public class ListRecursoTarefaQuery : IRequest<ListRecursoTarefaResponse>
    {
        public bool GetDependencies { get; set; }
    }
}
