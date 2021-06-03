using Cpnucleo.Infra.CrossCutting.Util.Commands.Responses.TipoTarefa;
using MediatR;
using System;
using System.Runtime.Serialization;

namespace Cpnucleo.Infra.CrossCutting.Util.Commands.Requests.TipoTarefa
{
    [DataContract]
    public class RemoveTipoTarefaCommand : IRequest<RemoveTipoTarefaResponse>
    {
        [DataMember(Order = 1)]
        public Guid Id { get; set; }
    }
}
