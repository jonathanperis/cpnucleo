namespace Cpnucleo.Infra.CrossCutting.Util.Commands.Workflow.RemoveWorkflow;

[DataContract]
public class RemoveWorkflowResponse
{
    [DataMember(Order = 1)]
    public OperationResult Status { get; set; }
}