using Cpnucleo.Infra.CrossCutting.Util.Commands.Responses.Impedimento;
using MediatR;
using System;
using System.Runtime.Serialization;

namespace Cpnucleo.Infra.CrossCutting.Util.Commands.Requests.Impedimento
{
    [DataContract]
    public class RemoveImpedimentoCommand : IRequest<RemoveImpedimentoResponse>
    {
        [DataMember(Order = 1)]
        public Guid Id { get; set; }
    }
}
