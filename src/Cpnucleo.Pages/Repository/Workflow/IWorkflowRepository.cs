using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cpnucleo.Pages.Repository.Workflow
{
    public interface IWorkflowRepository : IRepository<WorkflowItem>
    {
        Task<IEnumerable<WorkflowItem>> ListarTarefasWorkflow();        
    }
}