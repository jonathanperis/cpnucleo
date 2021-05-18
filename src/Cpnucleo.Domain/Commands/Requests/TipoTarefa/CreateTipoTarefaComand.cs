using Cpnucleo.Domain.Commands.Responses.TipoTarefa;
using MediatR;

namespace Cpnucleo.Domain.Commands.Requests.TipoTarefa
{
    public class CreateTipoTarefaComand : IRequest<CreateTipoTarefaResponse>
    {
        public Domain.Entities.TipoTarefa TipoTarefa { get; set; }
    }
}
