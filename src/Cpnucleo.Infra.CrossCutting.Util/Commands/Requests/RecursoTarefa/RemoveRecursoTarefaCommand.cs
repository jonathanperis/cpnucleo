using Cpnucleo.Infra.CrossCutting.Util.Commands.Responses.RecursoTarefa;
using MediatR;
using System;
using System.Runtime.Serialization;

namespace Cpnucleo.Infra.CrossCutting.Util.Commands.Requests.RecursoTarefa
{
    [DataContract]
    public class RemoveRecursoTarefaCommand : IRequest<RemoveRecursoTarefaResponse>
    {
        [DataMember(Order = 1)]
        public Guid Id { get; set; }
    }
}
