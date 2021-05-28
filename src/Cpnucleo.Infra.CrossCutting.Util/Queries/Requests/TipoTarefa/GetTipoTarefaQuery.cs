using Cpnucleo.Infra.CrossCutting.Util.Queries.Responses.TipoTarefa;
using MediatR;
using System;
using System.Runtime.Serialization;

namespace Cpnucleo.Infra.CrossCutting.Util.Queries.Requests.TipoTarefa
{
    [DataContract]
    public class GetTipoTarefaQuery : IRequest<GetTipoTarefaResponse>
    {
        [DataMember(Order = 1)]
        public Guid Id { get; set; }
    }
}
