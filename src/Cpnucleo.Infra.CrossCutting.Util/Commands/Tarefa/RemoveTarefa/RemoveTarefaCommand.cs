using Cpnucleo.Infra.CrossCutting.Util.Commands.Responses.Tarefa;
using MediatR;
using System;
using System.Runtime.Serialization;

namespace Cpnucleo.Infra.CrossCutting.Util.Commands.Requests.Tarefa
{
    [DataContract]
    public class RemoveTarefaCommand : IRequest<RemoveTarefaResponse>
    {
        [DataMember(Order = 1)]
        public Guid Id { get; set; }
    }
}
