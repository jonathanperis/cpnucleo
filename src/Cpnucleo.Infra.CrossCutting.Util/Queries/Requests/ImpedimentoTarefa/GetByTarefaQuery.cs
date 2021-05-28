using Cpnucleo.Infra.CrossCutting.Util.Queries.Responses.ImpedimentoTarefa;
using MediatR;
using System;
using System.Runtime.Serialization;

namespace Cpnucleo.Infra.CrossCutting.Util.Queries.Requests.ImpedimentoTarefa
{
    [DataContract]
    public class GetByTarefaQuery : IRequest<GetByTarefaResponse>
    {
        [DataMember(Order = 1)]
        public Guid IdTarefa { get; set; }
    }
}
