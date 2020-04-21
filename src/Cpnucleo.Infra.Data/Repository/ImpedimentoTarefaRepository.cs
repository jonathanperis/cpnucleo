using Cpnucleo.Domain.Entities;
using Cpnucleo.Domain.Interfaces.Repositories;
using Cpnucleo.Infra.Data.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Cpnucleo.Infra.Data.Repository
{
    internal class ImpedimentoTarefaRepository : CrudRepository<ImpedimentoTarefa>, IImpedimentoTarefaRepository
    {
        public ImpedimentoTarefaRepository(CpnucleoContext context)
            : base(context)
        {

        }

        public IEnumerable<ImpedimentoTarefa> ListarPorTarefa(Guid idTarefa)
        {
            return Listar()
                .Include(_context.GetIncludePaths(typeof(ImpedimentoTarefa)))
                .Where(x => x.IdTarefa == idTarefa)
                .ToList();
        }
    }
}
