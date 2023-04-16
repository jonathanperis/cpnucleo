namespace Cpnucleo.MVC.Models;

public sealed record WorkflowViewModel
{
    public WorkflowDto Workflow { get; set; }

    public IEnumerable<WorkflowDto> Lista { get; set; }
}
