using Cpnucleo.Domain.Queries.Responses.ImpedimentoTarefa;
using MediatR;

namespace Cpnucleo.Domain.Queries.Requests.ImpedimentoTarefa
{
    public class ListImpedimentoTarefaQuery : IRequest<ListImpedimentoTarefaResponse>
    {
        public bool GetDependencies { get; set; }
    }
}
