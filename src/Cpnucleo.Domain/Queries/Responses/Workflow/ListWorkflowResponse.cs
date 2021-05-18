using System.Collections.Generic;

namespace Cpnucleo.Domain.Queries.Responses.Workflow
{
    public class ListWorkflowResponse
    {
        public OperationResult Status { get; set; }
        public IEnumerable<Domain.Entities.Workflow> Workflows { get; set; }
    }
}
