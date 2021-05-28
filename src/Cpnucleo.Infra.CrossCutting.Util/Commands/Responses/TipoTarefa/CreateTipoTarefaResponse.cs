using Cpnucleo.Infra.CrossCutting.Util.ViewModels;

namespace Cpnucleo.Infra.CrossCutting.Util.Commands.Responses.TipoTarefa
{
    public class CreateTipoTarefaResponse
    {
        public OperationResult Status { get; set; }
        public TipoTarefaViewModel TipoTarefa { get; set; }
    }
}
