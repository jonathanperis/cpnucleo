using Cpnucleo.Infra.CrossCutting.Util.ViewModels;

namespace Cpnucleo.Infra.CrossCutting.Util.Commands.Responses.Projeto
{
    public class CreateProjetoResponse
    {
        public OperationResult Status { get; set; }
        public ProjetoViewModel Projeto { get; set; }
    }
}
