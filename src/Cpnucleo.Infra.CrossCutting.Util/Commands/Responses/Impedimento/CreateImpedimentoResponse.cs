using Cpnucleo.Infra.CrossCutting.Util.ViewModels;

namespace Cpnucleo.Infra.CrossCutting.Util.Commands.Responses.Impedimento
{
    public class CreateImpedimentoResponse
    {
        public OperationResult Status { get; set; }
        public ImpedimentoViewModel Impedimento { get; set; }
    }
}
