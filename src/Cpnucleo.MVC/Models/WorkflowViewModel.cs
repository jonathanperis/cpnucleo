using System.Collections.Generic;
using Cpnucleo.Domain.Entities;

namespace Cpnucleo.MVC.Models
{
    public class WorkflowViewModel
    {
        public Workflow Workflow { get; set; }

        public IEnumerable<Workflow> Lista { get; set; }
    }
}