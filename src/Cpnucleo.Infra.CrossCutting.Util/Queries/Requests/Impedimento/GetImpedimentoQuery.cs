using Cpnucleo.Infra.CrossCutting.Util.Queries.Responses.Impedimento;
using MediatR;
using System;
using System.Runtime.Serialization;

namespace Cpnucleo.Infra.CrossCutting.Util.Queries.Requests.Impedimento
{
    [DataContract]
    public class GetImpedimentoQuery : IRequest<GetImpedimentoResponse>
    {
        [DataMember(Order = 1)]
        public Guid Id { get; set; }
    }
}
