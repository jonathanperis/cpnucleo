using System.Runtime.Serialization;

namespace Cpnucleo.Infra.CrossCutting.Util.Commands.Responses.Apontamento
{
    [DataContract]
    public class UpdateApontamentoResponse
    {
        [DataMember(Order = 1)]
        public OperationResult Status { get; set; }
    }
}
