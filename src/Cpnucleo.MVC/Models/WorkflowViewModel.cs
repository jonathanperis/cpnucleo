using Cpnucleo.Shared.Common.DTOs;

namespace Cpnucleo.MVC.Models;

public class WorkflowViewModel
{
    public WorkflowDTO Workflow { get; set; }

    public IEnumerable<WorkflowDTO> Lista { get; set; }
}
