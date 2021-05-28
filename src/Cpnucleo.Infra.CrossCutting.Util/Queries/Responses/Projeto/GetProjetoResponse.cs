using Cpnucleo.Infra.CrossCutting.Util.ViewModels;

namespace Cpnucleo.Infra.CrossCutting.Util.Queries.Responses.Projeto
{
    public class GetProjetoResponse
    {
        public OperationResult Status { get; set; }
        public ProjetoViewModel Projeto { get; set; }
    }
}
