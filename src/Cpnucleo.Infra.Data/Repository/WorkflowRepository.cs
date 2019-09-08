using Cpnucleo.Domain.Interfaces;
using Cpnucleo.Domain.Models;
using Cpnucleo.Infra.Data.Context;
using System;
using System.Linq;

namespace Cpnucleo.Infra.Data.Repository
{
    public class WorkflowRepository : Repository<Workflow>, IWorkflowRepository
    {
        public WorkflowRepository(CpnucleoContext context)
            : base(context)
        {

        }

        public IQueryable<Workflow> ListarTarefasWorkflow()
        {
            throw new NotImplementedException();
        }
    }
}
