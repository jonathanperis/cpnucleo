using System.Runtime.Serialization;

namespace Cpnucleo.Infra.CrossCutting.Util.Commands.Responses.Tarefa
{
    [DataContract]
    public class UpdateByWorkflowResponse
    {
        [DataMember(Order = 1)]
        public OperationResult Status { get; set; }
    }
}
