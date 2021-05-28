using Cpnucleo.Infra.CrossCutting.Util.ViewModels;

namespace Cpnucleo.Infra.CrossCutting.Util.Queries.Responses.Tarefa
{
    public class GetTarefaResponse
    {
        public OperationResult Status { get; set; }
        public TarefaViewModel Tarefa { get; set; }
    }
}
