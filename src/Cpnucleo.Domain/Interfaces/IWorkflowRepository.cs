using Cpnucleo.Domain.Models;
using System.Collections.Generic;

namespace Cpnucleo.Domain.Interfaces
{
    public interface IWorkflowRepository : IRepository<Workflow>
    {
        IEnumerable<Workflow> ListarPorTarefa();        
    }
}