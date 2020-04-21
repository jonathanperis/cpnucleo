using Cpnucleo.Domain.Entities;
using Cpnucleo.Domain.Interfaces.Repositories;
using Cpnucleo.Infra.Data.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Cpnucleo.Infra.Data.Repository
{
    internal class RecursoProjetoRepository : CrudRepository<RecursoProjeto>, IRecursoProjetoRepository
    {
        public RecursoProjetoRepository(CpnucleoContext context)
            : base(context)
        {

        }

        public IEnumerable<RecursoProjeto> ListarPorProjeto(Guid idProjeto)
        {
            return Listar()
                .Include(_context.GetIncludePaths(typeof(RecursoProjeto)))
                .Where(x => x.IdProjeto == idProjeto)
                .ToList();
        }
    }
}
