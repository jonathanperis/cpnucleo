using Cpnucleo.Infra.CrossCutting.Util.Queries.Responses.Tarefa;
using MediatR;
using System;
using System.Runtime.Serialization;

namespace Cpnucleo.Infra.CrossCutting.Util.Queries.Requests.Tarefa
{
    [DataContract]
    public class GetByRecursoQuery : IRequest<GetByRecursoResponse>
    {
        [DataMember(Order = 1)]
        public Guid IdRecurso { get; set; }
    }
}
