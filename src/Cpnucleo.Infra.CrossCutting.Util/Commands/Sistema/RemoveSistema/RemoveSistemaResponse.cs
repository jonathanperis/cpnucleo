using System.Runtime.Serialization;

namespace Cpnucleo.Infra.CrossCutting.Util.Commands.Sistema.RemoveSistema
{
    [DataContract]
    public class RemoveSistemaResponse
    {
        [DataMember(Order = 1)]
        public OperationResult Status { get; set; }
    }
}
