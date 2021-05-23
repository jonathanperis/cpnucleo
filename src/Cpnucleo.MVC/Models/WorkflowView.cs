using System.Collections.Generic;
using Cpnucleo.Infra.CrossCutting.Util.ViewModels;

namespace Cpnucleo.MVC.Models
{
    public class WorkflowView
    {
        public WorkflowViewModel Workflow { get; set; }

        public IEnumerable<WorkflowViewModel> Lista { get; set; }
    }
}