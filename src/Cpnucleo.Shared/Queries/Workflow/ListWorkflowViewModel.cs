using Cpnucleo.Shared.Common.DTOs;
using Cpnucleo.Shared.Common.Models;

namespace Cpnucleo.Shared.Queries.Workflow;

public record ListWorkflowViewModel : BaseQuery
{
    public IEnumerable<WorkflowDTO> Workflows { get; set; }
    public OperationResult OperationResult { get; set; }
}
