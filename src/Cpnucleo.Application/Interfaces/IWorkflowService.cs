using Cpnucleo.Application.ViewModels;
using System.Collections.Generic;

namespace Cpnucleo.Application.Interfaces
{
    public interface IWorkflowAppService : ICrudAppService<WorkflowViewModel>
    {
        IEnumerable<WorkflowViewModel> ListarPorTarefa();
    }
}
