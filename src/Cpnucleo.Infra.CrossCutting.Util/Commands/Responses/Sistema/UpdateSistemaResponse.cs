using System.Runtime.Serialization;

namespace Cpnucleo.Infra.CrossCutting.Util.Commands.Responses.Sistema
{
    [DataContract]
    public class UpdateSistemaResponse
    {
        [DataMember(Order = 1)]
        public OperationResult Status { get; set; }
    }
}
