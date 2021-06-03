using Cpnucleo.Infra.CrossCutting.Util.Commands.Responses.Recurso;
using MediatR;
using System;
using System.Runtime.Serialization;

namespace Cpnucleo.Infra.CrossCutting.Util.Commands.Requests.Recurso
{
    [DataContract]
    public class RemoveRecursoCommand : IRequest<RemoveRecursoResponse>
    {
        [DataMember(Order = 1)]
        public Guid Id { get; set; }
    }
}
