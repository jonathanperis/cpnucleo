using Cpnucleo.Domain.Queries.Responses.TipoTarefa;
using MediatR;

namespace Cpnucleo.Domain.Queries.Requests.TipoTarefa
{
    public class ListTipoTarefaQuery : IRequest<ListTipoTarefaResponse>
    {
        public bool GetDependencies { get; set; }
    }
}
