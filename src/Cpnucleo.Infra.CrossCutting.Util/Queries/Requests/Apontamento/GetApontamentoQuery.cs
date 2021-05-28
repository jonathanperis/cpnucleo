using Cpnucleo.Infra.CrossCutting.Util.Queries.Responses.Apontamento;
using MediatR;
using System;
using System.Runtime.Serialization;

namespace Cpnucleo.Infra.CrossCutting.Util.Queries.Requests.Apontamento
{
    [DataContract]
    public class GetApontamentoQuery : IRequest<GetApontamentoResponse>
    {
        [DataMember(Order = 1)]
        public Guid Id { get; set; }
    }
}
