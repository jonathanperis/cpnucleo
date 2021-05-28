using Cpnucleo.Infra.CrossCutting.Util.ViewModels;

namespace Cpnucleo.Infra.CrossCutting.Util.Commands.Responses.RecursoTarefa
{
    public class CreateRecursoTarefaResponse
    {
        public OperationResult Status { get; set; }
        public RecursoTarefaViewModel RecursoTarefa { get; set; }
    }
}
