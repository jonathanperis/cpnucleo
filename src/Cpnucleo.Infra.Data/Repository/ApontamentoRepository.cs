using Cpnucleo.Domain.Entities;
using Cpnucleo.Domain.Interfaces.Repositories;
using Cpnucleo.Infra.Data.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Cpnucleo.Infra.Data.Repository
{
    internal class ApontamentoRepository : CrudRepository<Apontamento>, IApontamentoRepository
    {
        public ApontamentoRepository(CpnucleoContext context)
            : base(context)
        {

        }

        public IEnumerable<Apontamento> ListarPorRecurso(Guid idRecurso)
        {
            return Listar()
                .Include(_context.GetIncludePaths(typeof(Apontamento)))
                .Where(x => x.IdRecurso == idRecurso && x.DataApontamento.Value > DateTime.Now.AddDays(-30))
                .ToList();
        }

        public int ObterTotalHorasPorRecurso(Guid idRecurso, Guid idTarefa)
        {
            return Listar()
                .Include(_context.GetIncludePaths(typeof(Apontamento)))
                .Where(x => x.IdRecurso == idRecurso && x.IdTarefa == idTarefa)
                .Sum(x => x.QtdHoras);
        }
    }
}
