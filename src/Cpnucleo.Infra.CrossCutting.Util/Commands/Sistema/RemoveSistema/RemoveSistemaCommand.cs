using Cpnucleo.Infra.CrossCutting.Util.Commands.Responses.Sistema;
using MediatR;
using System;
using System.Runtime.Serialization;

namespace Cpnucleo.Infra.CrossCutting.Util.Commands.Requests.Sistema
{
    [DataContract]
    public class RemoveSistemaCommand : IRequest<RemoveSistemaResponse>
    {
        [DataMember(Order = 1)]
        public Guid Id { get; set; }
    }
}
