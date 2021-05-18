using Cpnucleo.Domain.Entities;
using Cpnucleo.Domain.Interfaces;
using Cpnucleo.Infra.Data.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cpnucleo.Infra.Data.Repositories
{
    internal class ImpedimentoTarefaRepository : GenericRepository<ImpedimentoTarefa>, IImpedimentoTarefaRepository
    {
        public ImpedimentoTarefaRepository(CpnucleoContext context)
            : base(context)
        {

        }

        public async Task<IEnumerable<ImpedimentoTarefa>> GetByTarefaAsync(Guid idTarefa)
        {
            IEnumerable<ImpedimentoTarefa> result = await AllAsync(true);

            return result
                .Where(x => x.IdTarefa == idTarefa)
                .ToList();
        }
    }
}
