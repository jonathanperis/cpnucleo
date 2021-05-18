using Cpnucleo.Domain.Entities;
using Cpnucleo.Domain.Repositories;
using Cpnucleo.Infra.Data.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cpnucleo.Infra.Data.Repositories
{
    internal class RecursoTarefaRepository : GenericRepository<RecursoTarefa>, IRecursoTarefaRepository
    {
        public RecursoTarefaRepository(CpnucleoContext context)
            : base(context)
        {

        }

        public async Task<IEnumerable<RecursoTarefa>> GetByTarefaAsync(Guid idTarefa)
        {
            IEnumerable<RecursoTarefa> result = await AllAsync(true);

            return result
                .Where(x => x.IdTarefa == idTarefa)
                .ToList();
        }
    }
}
