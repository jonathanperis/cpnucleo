using System.Collections.Generic;
using System.Threading.Tasks;

namespace dotnet_cpnucleo_pages.Repository.Workflow
{
    public interface IWorkflowRepository : IRepository<WorkflowItem>
    {
        Task<IList<WorkflowItem>> ListarTarefasWorkflow();        
    }
}