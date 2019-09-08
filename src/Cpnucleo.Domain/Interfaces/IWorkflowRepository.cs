using Cpnucleo.Domain.Models;
using System.Linq;

namespace Cpnucleo.Domain.Interfaces
{
    public interface IWorkflowRepository : IRepository<Workflow>
    {
        IQueryable<Workflow> ListarTarefasWorkflow();        
    }
}