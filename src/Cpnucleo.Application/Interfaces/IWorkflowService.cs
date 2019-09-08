using Cpnucleo.Application.ViewModels;
using System.Linq;

namespace Cpnucleo.Application.Interfaces
{
    public interface IWorkflowAppService : IAppService<WorkflowViewModel>
    {
        IQueryable<WorkflowViewModel> ListarTarefasWorkflow();
    }
}
