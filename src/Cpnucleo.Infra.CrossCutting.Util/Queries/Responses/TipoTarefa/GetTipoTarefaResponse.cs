using Cpnucleo.Infra.CrossCutting.Util.ViewModels;

namespace Cpnucleo.Infra.CrossCutting.Util.Queries.Responses.TipoTarefa
{
    public class GetTipoTarefaResponse
    {
        public OperationResult Status { get; set; }
        public TipoTarefaViewModel TipoTarefa { get; set; }
    }
}
