using Cpnucleo.Infra.CrossCutting.Util.Commands.Responses.Apontamento;
using MediatR;
using System;
using System.Runtime.Serialization;

namespace Cpnucleo.Infra.CrossCutting.Util.Commands.Requests.Apontamento
{
    [DataContract]
    public class RemoveApontamentoCommand : IRequest<RemoveApontamentoResponse>
    {
        [DataMember(Order = 1)]
        public Guid Id { get; set; }
    }
}
