using Cpnucleo.Domain.Commands.Responses.ImpedimentoTarefa;
using MediatR;

namespace Cpnucleo.Domain.Commands.Requests.ImpedimentoTarefa
{
    public class CreateImpedimentoTarefaComand : IRequest<CreateImpedimentoTarefaResponse>
    {
        public Domain.Entities.ImpedimentoTarefa ImpedimentoTarefa { get; set; }
    }
}
