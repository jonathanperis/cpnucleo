using Cpnucleo.Infra.CrossCutting.Util.ViewModels;
using System.Runtime.Serialization;

namespace Cpnucleo.Infra.CrossCutting.Util.Queries.Responses.ImpedimentoTarefa
{
    [DataContract]
    public class GetImpedimentoTarefaResponse
    {
        [DataMember(Order = 1)]
        public OperationResult Status { get; set; }

        [DataMember(Order = 2)]
        public ImpedimentoTarefaViewModel ImpedimentoTarefa { get; set; }
    }
}
