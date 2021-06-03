using System.Runtime.Serialization;

namespace Cpnucleo.Infra.CrossCutting.Util.Commands.Impedimento.UpdateImpedimento
{
    [DataContract]
    public class UpdateImpedimentoResponse
    {
        [DataMember(Order = 1)]
        public OperationResult Status { get; set; }
    }
}
