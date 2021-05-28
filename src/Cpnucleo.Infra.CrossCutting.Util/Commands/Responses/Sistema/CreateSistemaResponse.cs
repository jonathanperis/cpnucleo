using Cpnucleo.Infra.CrossCutting.Util.ViewModels;

namespace Cpnucleo.Infra.CrossCutting.Util.Commands.Responses.Sistema
{
    public class CreateSistemaResponse
    {
        public OperationResult Status { get; set; }
        public SistemaViewModel Sistema { get; set; }
    }
}
