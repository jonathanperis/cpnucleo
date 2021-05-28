using Cpnucleo.Infra.CrossCutting.Util.ViewModels;

namespace Cpnucleo.Infra.CrossCutting.Util.Queries.Responses.Apontamento
{
    public class GetApontamentoResponse
    {
        public OperationResult Status { get; set; }
        public ApontamentoViewModel Apontamento { get; set; }
    }
}
