using Cpnucleo.Infra.CrossCutting.Util.Queries.Responses.ImpedimentoTarefa;
using MediatR;
using System.Runtime.Serialization;

namespace Cpnucleo.Infra.CrossCutting.Util.Queries.Requests.ImpedimentoTarefa
{
    [DataContract]
    public class ListImpedimentoTarefaQuery : IRequest<ListImpedimentoTarefaResponse>
    {
        [DataMember(Order = 1)]
        public bool GetDependencies { get; set; }
    }
}
