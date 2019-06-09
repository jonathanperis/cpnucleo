using Cpnucleo.Pages.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cpnucleo.Pages.Repository
{
    public interface IWorkflowRepository : IRepository<WorkflowItem>
    {
        Task<IEnumerable<WorkflowItem>> ListarTarefasWorkflowAsync();        
    }
}