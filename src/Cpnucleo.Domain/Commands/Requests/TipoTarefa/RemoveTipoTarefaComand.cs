using Cpnucleo.Domain.Commands.Responses.TipoTarefa;
using MediatR;
using System;

namespace Cpnucleo.Domain.Commands.Requests.TipoTarefa
{
    public class RemoveTipoTarefaComand : IRequest<RemoveTipoTarefaResponse>
    {
        public Guid Id { get; set; }
    }
}
