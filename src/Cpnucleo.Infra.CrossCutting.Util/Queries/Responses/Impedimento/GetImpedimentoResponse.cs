using Cpnucleo.Infra.CrossCutting.Util.ViewModels;

namespace Cpnucleo.Infra.CrossCutting.Util.Queries.Responses.Impedimento
{
    public class GetImpedimentoResponse
    {
        public OperationResult Status { get; set; }
        public ImpedimentoViewModel Impedimento { get; set; }
    }
}
