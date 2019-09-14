using Cpnucleo.Application.ViewModels;
using System.Collections.Generic;

namespace Cpnucleo.Application.Interfaces
{
    public interface IWorkflowAppService : IAppService<WorkflowViewModel>
    {
        IEnumerable<WorkflowViewModel> ListarPorTarefa();
    }
}
