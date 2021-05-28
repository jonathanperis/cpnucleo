using Cpnucleo.Infra.CrossCutting.Util.Commands.Responses.ImpedimentoTarefa;
using MediatR;
using System;

namespace Cpnucleo.Infra.CrossCutting.Util.Commands.Requests.ImpedimentoTarefa
{
    public class RemoveImpedimentoTarefaCommand : IRequest<RemoveImpedimentoTarefaResponse>
    {
        public Guid Id { get; set; }
    }
}
