using System.Runtime.Serialization;

namespace Cpnucleo.Infra.CrossCutting.Util.Commands.Responses.Impedimento
{
    [DataContract]
    public class UpdateImpedimentoResponse
    {
        [DataMember(Order = 1)]
        public OperationResult Status { get; set; }
    }
}
