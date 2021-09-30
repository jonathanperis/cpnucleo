namespace Cpnucleo.Infra.CrossCutting.Util.Queries.Workflow.ListWorkflow;

[DataContract]
public class ListWorkflowQuery
{
    [DataMember(Order = 1)]
    public bool GetDependencies { get; set; }
}