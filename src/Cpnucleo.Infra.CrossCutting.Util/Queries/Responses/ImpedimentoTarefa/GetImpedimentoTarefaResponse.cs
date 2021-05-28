using Cpnucleo.Infra.CrossCutting.Util.ViewModels;

namespace Cpnucleo.Infra.CrossCutting.Util.Queries.Responses.ImpedimentoTarefa
{
    public class GetImpedimentoTarefaResponse
    {
        public OperationResult Status { get; set; }
        public ImpedimentoTarefaViewModel ImpedimentoTarefa { get; set; }
    }
}
