using Cpnucleo.Infra.CrossCutting.Util.ViewModels;

namespace Cpnucleo.Infra.CrossCutting.Util.Commands.Responses.RecursoProjeto
{
    public class CreateRecursoProjetoResponse
    {
        public OperationResult Status { get; set; }
        public RecursoProjetoViewModel RecursoProjeto { get; set; }
    }
}
