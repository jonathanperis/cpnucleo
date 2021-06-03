using System.Runtime.Serialization;

namespace Cpnucleo.Infra.CrossCutting.Util.Commands.Responses.ImpedimentoTarefa
{
    [DataContract]
    public class RemoveImpedimentoTarefaResponse
    {
        [DataMember(Order = 1)]
        public OperationResult Status { get; set; }
    }
}
