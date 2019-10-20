using Cpnucleo.Domain.Interfaces;
using Cpnucleo.Domain.Models;
using Cpnucleo.Infra.Data.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Cpnucleo.Infra.Data.Repository
{
    public class ImpedimentoTarefaRepository : CrudRepository<ImpedimentoTarefa>, IImpedimentoTarefaRepository
    {
        public ImpedimentoTarefaRepository(CpnucleoContext context)
            : base(context)
        {

        }

        public IEnumerable<ImpedimentoTarefa> ListarPorTarefa(Guid idTarefa)
        {
            return DbSet
                .AsNoTracking()
                .Include(x => x.Tarefa)
                .Include(x => x.Impedimento)
                .OrderBy(x => x.DataInclusao)
                .Where(x => x.IdTarefa == idTarefa && x.Ativo)
                .ToList();
        }
    }
}
