using Cpnucleo.Domain.Entities;
using Cpnucleo.Domain.Interfaces.Repositories;
using Cpnucleo.Infra.Data.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Cpnucleo.Infra.Data.Repository
{
    internal class RecursoTarefaRepository : CrudRepository<RecursoTarefa>, IRecursoTarefaRepository
    {
        public RecursoTarefaRepository(CpnucleoContext context)
            : base(context)
        {

        }

        public IEnumerable<RecursoTarefa> ListarPorTarefa(Guid idTarefa)
        {
            return Listar()
                .Include(_context.GetIncludePaths(typeof(RecursoTarefa)))
                .Where(x => x.IdTarefa == idTarefa)
                .ToList();
        }
    }
}
