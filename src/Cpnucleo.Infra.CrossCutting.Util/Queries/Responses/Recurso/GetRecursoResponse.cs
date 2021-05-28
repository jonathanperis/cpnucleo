using Cpnucleo.Infra.CrossCutting.Util.ViewModels;

namespace Cpnucleo.Infra.CrossCutting.Util.Queries.Responses.Recurso
{
    public class GetRecursoResponse
    {
        public OperationResult Status { get; set; }
        public RecursoViewModel Recurso { get; set; }
    }
}
