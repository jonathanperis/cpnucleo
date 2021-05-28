using Cpnucleo.Infra.CrossCutting.Util.Commands.Responses.TipoTarefa;
using Cpnucleo.Infra.CrossCutting.Util.ViewModels;
using MediatR;

namespace Cpnucleo.Infra.CrossCutting.Util.Commands.Requests.TipoTarefa
{
    public class UpdateTipoTarefaCommand : IRequest<UpdateTipoTarefaResponse>
    {
        public TipoTarefaViewModel TipoTarefa { get; set; }
    }
}
