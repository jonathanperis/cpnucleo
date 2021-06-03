using MediatR;
using System;
using System.Runtime.Serialization;

namespace Cpnucleo.Infra.CrossCutting.Util.Queries.RecursoTarefa.GetByTarefa
{
    [DataContract]
    public class GetByTarefaQuery : IRequest<GetByTarefaResponse>
    {
        [DataMember(Order = 1)]
        public Guid IdTarefa { get; set; }
    }
}
