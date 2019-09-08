using Cpnucleo.Domain.Interfaces;
using Cpnucleo.Domain.Models;
using Cpnucleo.Infra.Data.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Cpnucleo.Infra.Data.Repository
{
    public class RecursoProjetoRepository : Repository<RecursoProjeto>, IRecursoProjetoRepository
    {
        public RecursoProjetoRepository(CpnucleoContext context)
            : base(context)
        {

        }

        public IEnumerable<RecursoProjeto> ListarPoridProjeto(Guid idProjeto)
        {
            return DbSet
                .AsNoTracking()
                .Include(x => x.Projeto)
                .Include(x => x.Recurso)
                .OrderBy(x => x.DataInclusao)
                .Where(x => x.IdProjeto == idProjeto)
                .ToList();
        }
    }
}
