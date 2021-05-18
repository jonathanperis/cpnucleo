using Cpnucleo.Domain.Entities;
using Cpnucleo.Domain.Interfaces;
using Cpnucleo.Infra.Data.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cpnucleo.Infra.Data.Repositories
{
    internal class ApontamentoRepository : GenericRepository<Apontamento>, IApontamentoRepository
    {
        public ApontamentoRepository(CpnucleoContext context)
            : base(context)
        {

        }

        public async Task<IEnumerable<Apontamento>> GetByRecursoAsync(Guid idRecurso)
        {
            IEnumerable<Apontamento> result = await AllAsync(true);

            return result
                .Where(x => x.IdRecurso == idRecurso && x.DataApontamento.Value > DateTime.Now.AddDays(-30))
                .ToList();
        }

        public async Task<int> GetTotalHorasPorRecursoAsync(Guid idRecurso, Guid idTarefa)
        {
            IEnumerable<Apontamento> result = await AllAsync(true);

            return result
                .Where(x => x.IdRecurso == idRecurso && x.IdTarefa == idTarefa)
                .Sum(x => x.QtdHoras);
        }
    }
}
