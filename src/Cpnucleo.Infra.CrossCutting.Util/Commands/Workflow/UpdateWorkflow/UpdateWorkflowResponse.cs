namespace Cpnucleo.Infra.CrossCutting.Util.Commands.Workflow.UpdateWorkflow
{
    [DataContract]
    public class UpdateWorkflowResponse
    {
        [DataMember(Order = 1)]
        public OperationResult Status { get; set; }
    }
}
