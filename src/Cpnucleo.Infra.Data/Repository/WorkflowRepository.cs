using Cpnucleo.Domain.Interfaces;
using Cpnucleo.Domain.Models;
using Cpnucleo.Infra.Data.Context;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace Cpnucleo.Infra.Data.Repository
{
    public class WorkflowRepository : Repository<Workflow>, IWorkflowRepository
    {
        public WorkflowRepository(CpnucleoContext context)
            : base(context)
        {

        }

        public IEnumerable<Workflow> ListarPorTarefa()
        {
            return DbSet
                .AsNoTracking()
                .Include(x => x.ListaTarefas)
                    .ThenInclude(x => x.Recurso)
                .Include(x => x.ListaTarefas)
                    .ThenInclude(x => x.TipoTarefa)
                .Include(x => x.ListaTarefas)
                    .ThenInclude(x => x.ListaApontamentos)
                .Include(x => x.ListaTarefas)
                    .ThenInclude(x => x.ListaImpedimentos)
                .OrderBy(x => x.Ordem)
                .Take(4) //@@JONATHAN - 22/02/2017 - TRAVA TEMPORÁRIA.
                .ToList();
        }
    }
}
