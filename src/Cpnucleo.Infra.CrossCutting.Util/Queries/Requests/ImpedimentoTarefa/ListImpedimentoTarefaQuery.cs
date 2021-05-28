using Cpnucleo.Infra.CrossCutting.Util.Queries.Responses.ImpedimentoTarefa;
using MediatR;

namespace Cpnucleo.Infra.CrossCutting.Util.Queries.Requests.ImpedimentoTarefa
{
    public class ListImpedimentoTarefaQuery : IRequest<ListImpedimentoTarefaResponse>
    {
        public bool GetDependencies { get; set; }
    }
}
