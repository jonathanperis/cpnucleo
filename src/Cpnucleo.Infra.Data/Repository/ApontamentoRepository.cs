using Cpnucleo.Domain.Interfaces;
using Cpnucleo.Domain.Models;
using Cpnucleo.Infra.Data.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Cpnucleo.Infra.Data.Repository
{
    public class ApontamentoRepository : Repository<Apontamento>, IApontamentoRepository
    {
        public ApontamentoRepository(CpnucleoContext context)
            : base(context)
        {

        }

        public IEnumerable<Apontamento> ListarPoridRecurso(Guid idRecurso)
        {
            return DbSet
                    .AsNoTracking()
                    .Include(x => x.Tarefa)
                    .OrderBy(x => x.DataInclusao)
                    .Where(x => x.IdRecurso == idRecurso && x.DataApontamento.Value > DateTime.Now.AddDays(-30))
                    .ToList();
        }

        public int ObterTotalHorasPoridRecurso(Guid idRecurso, Guid idTarefa)
        {
            return DbSet
                .AsNoTracking()
                .Include(x => x.Tarefa)
                .OrderBy(x => x.DataInclusao)
                .Where(x => x.IdRecurso == idRecurso && x.IdTarefa == idTarefa)
                .Sum(x => x.QtdHoras);
        }
    }
}
