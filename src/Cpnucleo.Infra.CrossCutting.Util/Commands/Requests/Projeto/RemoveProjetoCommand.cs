using Cpnucleo.Infra.CrossCutting.Util.Commands.Responses.Projeto;
using MediatR;
using System;
using System.Runtime.Serialization;

namespace Cpnucleo.Infra.CrossCutting.Util.Commands.Requests.Projeto
{
    [DataContract]
    public class RemoveProjetoCommand : IRequest<RemoveProjetoResponse>
    {
        [DataMember(Order = 1)]
        public Guid Id { get; set; }
    }
}
