using Cpnucleo.Infra.CrossCutting.Util.Queries.Responses.Tarefa;
using MediatR;

namespace Cpnucleo.Infra.CrossCutting.Util.Queries.Requests.Tarefa
{
    public class ListTarefaQuery : IRequest<ListTarefaResponse>
    {
        public bool GetDependencies { get; set; }
    }
}
