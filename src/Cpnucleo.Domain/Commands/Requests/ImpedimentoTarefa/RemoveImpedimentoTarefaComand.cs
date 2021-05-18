using Cpnucleo.Domain.Commands.Responses.ImpedimentoTarefa;
using MediatR;
using System;

namespace Cpnucleo.Domain.Commands.Requests.ImpedimentoTarefa
{
    public class RemoveImpedimentoTarefaComand : IRequest<RemoveImpedimentoTarefaResponse>
    {
        public Guid Id { get; set; }
    }
}
