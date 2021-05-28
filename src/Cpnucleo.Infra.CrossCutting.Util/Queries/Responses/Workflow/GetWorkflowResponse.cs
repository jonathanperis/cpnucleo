using Cpnucleo.Infra.CrossCutting.Util.ViewModels;

namespace Cpnucleo.Infra.CrossCutting.Util.Queries.Responses.Workflow
{
    public class GetWorkflowResponse
    {
        public OperationResult Status { get; set; }
        public WorkflowViewModel Workflow { get; set; }
    }
}
