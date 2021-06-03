using MediatR;
using System;
using System.Runtime.Serialization;

namespace Cpnucleo.Infra.CrossCutting.Util.Queries.Apontamento.GetApontamento
{
    [DataContract]
    public class GetApontamentoQuery : IRequest<GetApontamentoResponse>
    {
        [DataMember(Order = 1)]
        public Guid Id { get; set; }
    }
}
