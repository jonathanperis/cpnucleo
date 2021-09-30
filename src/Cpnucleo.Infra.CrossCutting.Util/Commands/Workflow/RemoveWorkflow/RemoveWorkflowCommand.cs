namespace Cpnucleo.Infra.CrossCutting.Util.Commands.Workflow.RemoveWorkflow
{
    [DataContract]
    public class RemoveWorkflowCommand
    {
        [DataMember(Order = 1)]
        public Guid Id { get; set; }
    }
}
