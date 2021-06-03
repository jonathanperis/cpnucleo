using Cpnucleo.Infra.CrossCutting.Util.Commands.Responses.RecursoProjeto;
using MediatR;
using System;
using System.Runtime.Serialization;

namespace Cpnucleo.Infra.CrossCutting.Util.Commands.Requests.RecursoProjeto
{
    [DataContract]
    public class RemoveRecursoProjetoCommand : IRequest<RemoveRecursoProjetoResponse>
    {
        [DataMember(Order = 1)]
        public Guid Id { get; set; }
    }
}
