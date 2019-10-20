using Cpnucleo.Domain.Models;
using System.Linq;

namespace Cpnucleo.Domain.Interfaces
{
    public interface IWorkflowRepository : ICrudRepository<Workflow>
    {
        IQueryable<Workflow> ListarPorTarefa();
    }
}