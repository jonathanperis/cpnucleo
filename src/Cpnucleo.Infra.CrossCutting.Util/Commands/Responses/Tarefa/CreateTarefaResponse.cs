using Cpnucleo.Infra.CrossCutting.Util.ViewModels;

namespace Cpnucleo.Infra.CrossCutting.Util.Commands.Responses.Tarefa
{
    public class CreateTarefaResponse
    {
        public OperationResult Status { get; set; }
        public TarefaViewModel Tarefa { get; set; }
    }
}
