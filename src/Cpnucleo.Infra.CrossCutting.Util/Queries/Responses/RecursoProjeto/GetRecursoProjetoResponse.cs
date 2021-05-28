using Cpnucleo.Infra.CrossCutting.Util.ViewModels;

namespace Cpnucleo.Infra.CrossCutting.Util.Queries.Responses.RecursoProjeto
{
    public class GetRecursoProjetoResponse
    {
        public OperationResult Status { get; set; }
        public RecursoProjetoViewModel RecursoProjeto { get; set; }
    }
}
