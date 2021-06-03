using Cpnucleo.Infra.CrossCutting.Util.Queries.Responses.RecursoTarefa;
using MediatR;
using System;
using System.Runtime.Serialization;

namespace Cpnucleo.Infra.CrossCutting.Util.Queries.Requests.RecursoTarefa
{
    [DataContract]
    public class GetRecursoTarefaQuery : IRequest<GetRecursoTarefaResponse>
    {
        [DataMember(Order = 1)]
        public Guid Id { get; set; }
    }
}
