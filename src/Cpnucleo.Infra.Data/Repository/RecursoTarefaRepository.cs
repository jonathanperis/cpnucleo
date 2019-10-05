using Cpnucleo.Domain.Interfaces;
using Cpnucleo.Domain.Models;
using Cpnucleo.Infra.Data.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Cpnucleo.Infra.Data.Repository
{
    public class RecursoTarefaRepository : CrudRepository<RecursoTarefa>, IRecursoTarefaRepository
    {
        public RecursoTarefaRepository(CpnucleoContext context)
            : base(context)
        {

        }

        public IEnumerable<RecursoTarefa> ListarPorRecurso(Guid idRecurso)
        {
            return DbSet
                .AsNoTracking()
                .Include(x => x.Tarefa)
                    .ThenInclude(x => x.TipoTarefa)
                .Include(x => x.Tarefa.Workflow)
                .Include(x => x.Tarefa.Recurso)
                .Include(x => x.Tarefa.ListaImpedimentos)
                .OrderBy(x => x.DataInclusao)
                .Where(x => x.IdRecurso == idRecurso)
                .ToList();
        }

        public IEnumerable<RecursoTarefa> ListarPorTarefa(Guid idTarefa)
        {
            return DbSet
                .AsNoTracking()
                .Include(x => x.Tarefa)
                .Include(x => x.Recurso)
                .OrderBy(x => x.DataInclusao)
                .Where(x => x.IdTarefa == idTarefa)
                .ToList();
        }
    }
}
