using Cpnucleo.Infra.CrossCutting.Util.ViewModels;

namespace Cpnucleo.Infra.CrossCutting.Util.Commands.Responses.ImpedimentoTarefa
{
    public class CreateImpedimentoTarefaResponse
    {
        public OperationResult Status { get; set; }
        public ImpedimentoTarefaViewModel ImpedimentoTarefa { get; set; }
    }
}
