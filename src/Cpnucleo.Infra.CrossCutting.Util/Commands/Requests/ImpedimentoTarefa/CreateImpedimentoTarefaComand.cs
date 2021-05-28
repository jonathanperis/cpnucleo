using Cpnucleo.Infra.CrossCutting.Util.Commands.Responses.ImpedimentoTarefa;
using Cpnucleo.Infra.CrossCutting.Util.ViewModels;
using MediatR;

namespace Cpnucleo.Infra.CrossCutting.Util.Commands.Requests.ImpedimentoTarefa
{
    public class CreateImpedimentoTarefaComand : IRequest<CreateImpedimentoTarefaResponse>
    {
        public ImpedimentoTarefaViewModel ImpedimentoTarefa { get; set; }
    }
}
