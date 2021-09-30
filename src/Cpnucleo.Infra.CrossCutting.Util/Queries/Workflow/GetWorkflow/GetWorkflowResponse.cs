namespace Cpnucleo.Infra.CrossCutting.Util.Queries.Workflow.GetWorkflow
{
    [DataContract]
    public class GetWorkflowResponse
    {
        [DataMember(Order = 1)]
        public OperationResult Status { get; set; }

        [DataMember(Order = 2)]
        public WorkflowViewModel Workflow { get; set; }
    }
}
