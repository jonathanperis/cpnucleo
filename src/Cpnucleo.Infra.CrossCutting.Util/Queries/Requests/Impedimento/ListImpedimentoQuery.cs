using Cpnucleo.Infra.CrossCutting.Util.Queries.Responses.Impedimento;
using MediatR;
using System.Runtime.Serialization;

namespace Cpnucleo.Infra.CrossCutting.Util.Queries.Requests.Impedimento
{
    [DataContract]
    public class ListImpedimentoQuery : IRequest<ListImpedimentoResponse>
    {
        [DataMember(Order = 1)]
        public bool GetDependencies { get; set; }
    }
}
