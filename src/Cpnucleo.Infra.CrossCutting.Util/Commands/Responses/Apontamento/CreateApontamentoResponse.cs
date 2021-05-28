using Cpnucleo.Infra.CrossCutting.Util.ViewModels;

namespace Cpnucleo.Infra.CrossCutting.Util.Commands.Responses.Apontamento
{
    public class CreateApontamentoResponse
    {
        public OperationResult Status { get; set; }
        public ApontamentoViewModel Apontamento { get; set; }
    }
}
