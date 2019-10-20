using Cpnucleo.Domain.Interfaces;
using Cpnucleo.Domain.Models;
using Cpnucleo.Infra.Data.Context;
using System.Linq;

namespace Cpnucleo.Infra.Data.Repository
{
    public class WorkflowRepository : CrudRepository<Workflow>, IWorkflowRepository
    {
        public WorkflowRepository(CpnucleoContext context)
            : base(context)
        {

        }

        public IQueryable<Workflow> ListarPorTarefa()
        {
            return Listar()
                .OrderBy(x => x.Ordem)
                .Take(4); //@@JONATHAN - 22/02/2017 - TRAVA TEMPORÁRIA.
        }
    }
}
