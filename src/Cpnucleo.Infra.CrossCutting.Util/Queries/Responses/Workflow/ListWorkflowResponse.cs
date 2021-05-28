using Cpnucleo.Infra.CrossCutting.Util.ViewModels;
using System.Collections.Generic;

namespace Cpnucleo.Infra.CrossCutting.Util.Queries.Responses.Workflow
{
    public class ListWorkflowResponse
    {
        public OperationResult Status { get; set; }
        public IEnumerable<WorkflowViewModel> Workflows { get; set; }
    }
}
