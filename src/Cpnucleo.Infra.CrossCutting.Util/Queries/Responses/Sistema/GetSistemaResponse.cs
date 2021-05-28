using Cpnucleo.Infra.CrossCutting.Util.ViewModels;

namespace Cpnucleo.Infra.CrossCutting.Util.Queries.Responses.Sistema
{
    public class GetSistemaResponse
    {
        public OperationResult Status { get; set; }
        public SistemaViewModel Sistema { get; set; }
    }
}
