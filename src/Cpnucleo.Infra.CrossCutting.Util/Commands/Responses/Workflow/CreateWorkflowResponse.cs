using Cpnucleo.Infra.CrossCutting.Util.ViewModels;

namespace Cpnucleo.Infra.CrossCutting.Util.Commands.Responses.Workflow
{
    public class CreateWorkflowResponse
    {
        public OperationResult Status { get; set; }
        public WorkflowViewModel Workflow { get; set; }
    }
}
