using Cpnucleo.Infra.CrossCutting.Util.ViewModels;

namespace Cpnucleo.Infra.CrossCutting.Util.Queries.Responses.RecursoTarefa
{
    public class GetRecursoTarefaResponse
    {
        public OperationResult Status { get; set; }
        public RecursoTarefaViewModel RecursoTarefa { get; set; }
    }
}
