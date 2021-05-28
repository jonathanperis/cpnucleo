using Cpnucleo.Infra.CrossCutting.Util.Commands.Responses.ImpedimentoTarefa;
using MediatR;
using System;

namespace Cpnucleo.Infra.CrossCutting.Util.Commands.Requests.ImpedimentoTarefa
{
    public class RemoveImpedimentoTarefaComand : IRequest<RemoveImpedimentoTarefaResponse>
    {
        public Guid Id { get; set; }
    }
}
