using Cpnucleo.Domain.Entities;
using Cpnucleo.Domain.Interfaces;
using Cpnucleo.Infra.Data.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cpnucleo.Infra.Data.Repositories
{
    internal class RecursoProjetoRepository : GenericRepository<RecursoProjeto>, IRecursoProjetoRepository
    {
        public RecursoProjetoRepository(CpnucleoContext context)
            : base(context)
        {

        }

        public async Task<IEnumerable<RecursoProjeto>> GetByProjetoAsync(Guid idProjeto)
        {
            IEnumerable<RecursoProjeto> result = await AllAsync(true);

            return result
                .Where(x => x.IdProjeto == idProjeto)
                .ToList();
        }
    }
}
