using System.Runtime.Serialization;

namespace Cpnucleo.Infra.CrossCutting.Util.Commands.Responses.Workflow
{
    [DataContract]
    public class RemoveWorkflowResponse
    {
        [DataMember(Order = 1)]
        public OperationResult Status { get; set; }
    }
}
