using Cpnucleo.Infra.CrossCutting.Util.ViewModels;

namespace Cpnucleo.Infra.CrossCutting.Util.Commands.Responses.Recurso
{
    public class CreateRecursoResponse
    {
        public OperationResult Status { get; set; }
        public RecursoViewModel Recurso { get; set; }
    }
}
