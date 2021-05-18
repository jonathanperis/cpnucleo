using Cpnucleo.Domain.Queries.Responses.Tarefa;
using MediatR;

namespace Cpnucleo.Domain.Queries.Requests.Tarefa
{
    public class ListTarefaQuery : IRequest<ListTarefaResponse>
    {
        public bool GetDependencies { get; set; }
    }
}
