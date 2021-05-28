using Cpnucleo.Infra.CrossCutting.Util.Queries.Responses.TipoTarefa;
using MediatR;

namespace Cpnucleo.Infra.CrossCutting.Util.Queries.Requests.TipoTarefa
{
    public class ListTipoTarefaQuery : IRequest<ListTipoTarefaResponse>
    {
        public bool GetDependencies { get; set; }
    }
}
