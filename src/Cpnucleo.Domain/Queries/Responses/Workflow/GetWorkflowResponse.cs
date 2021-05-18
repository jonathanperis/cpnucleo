namespace Cpnucleo.Domain.Queries.Responses.Workflow
{
    public class GetWorkflowResponse
    {
        public OperationResult Status { get; set; }
        public Domain.Entities.Workflow Workflow { get; set; }
    }
}
