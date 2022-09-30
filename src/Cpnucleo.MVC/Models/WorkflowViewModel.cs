namespace Cpnucleo.MVC.Models;

public sealed record WorkflowViewModel
{
    public WorkflowDTO Workflow { get; set; }

    public IEnumerable<WorkflowDTO> Lista { get; set; }
}
