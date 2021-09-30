namespace Cpnucleo.Infra.CrossCutting.Util.Commands.Workflow.CreateWorkflow;

[DataContract]
public class CreateWorkflowResponse
{
    [DataMember(Order = 1)]
    public OperationResult Status { get; set; }

    [DataMember(Order = 2)]
    public WorkflowViewModel Workflow { get; set; }
}