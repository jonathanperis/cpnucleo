using MediatR;
using System;
using System.Runtime.Serialization;

namespace Cpnucleo.Infra.CrossCutting.Util.Queries.Impedimento.GetImpedimento
{
    [DataContract]
    public class GetImpedimentoQuery : IRequest<GetImpedimentoResponse>
    {
        [DataMember(Order = 1)]
        public Guid Id { get; set; }
    }
}
