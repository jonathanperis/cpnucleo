using System.Runtime.Serialization;

namespace Cpnucleo.Infra.CrossCutting.Util.Commands.Responses.Projeto
{
    [DataContract]
    public class UpdateProjetoResponse
    {
        [DataMember(Order = 1)]
        public OperationResult Status { get; set; }
    }
}
