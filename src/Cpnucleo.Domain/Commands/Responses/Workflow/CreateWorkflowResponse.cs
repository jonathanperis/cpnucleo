namespace Cpnucleo.Domain.Commands.Responses.Workflow
{
    public class CreateWorkflowResponse
    {
        public OperationResult Status { get; set; }
        public Domain.Entities.Workflow Workflow { get; set; }
    }
}
