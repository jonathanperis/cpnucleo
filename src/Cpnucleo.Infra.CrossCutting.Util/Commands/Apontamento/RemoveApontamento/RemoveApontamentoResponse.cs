using System.Runtime.Serialization;

namespace Cpnucleo.Infra.CrossCutting.Util.Commands.Apontamento.RemoveApontamento
{
    [DataContract]
    public class RemoveApontamentoResponse
    {
        [DataMember(Order = 1)]
        public OperationResult Status { get; set; }
    }
}
