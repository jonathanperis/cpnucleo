using MediatR;
using System.Runtime.Serialization;

namespace Cpnucleo.Infra.CrossCutting.Util.Queries.Impedimento.ListImpedimento
{
    [DataContract]
    public class ListImpedimentoQuery : IRequest<ListImpedimentoResponse>
    {
        [DataMember(Order = 1)]
        public bool GetDependencies { get; set; }
    }
}
