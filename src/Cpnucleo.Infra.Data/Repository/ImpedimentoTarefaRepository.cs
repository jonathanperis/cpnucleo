using Cpnucleo.Domain.Interfaces;
using Cpnucleo.Domain.Models;
using Cpnucleo.Infra.Data.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace Cpnucleo.Infra.Data.Repository
{
    public class ImpedimentoTarefaRepository : CrudRepository<ImpedimentoTarefa>, IImpedimentoTarefaRepository
    {
        public ImpedimentoTarefaRepository(CpnucleoContext context)
            : base(context)
        {

        }

        public IQueryable<ImpedimentoTarefa> ConsultaPorTarefa(Guid idTarefa)
        {
            return _dbSet
                .AsNoTracking()
                .Where(x => x.IdTarefa == idTarefa && x.Ativo);
        }

        public IQueryable<ImpedimentoTarefa> ListarPorTarefa(Guid idTarefa)
        {
            return Listar()
                .Where(x => x.IdTarefa == idTarefa);
        }
    }
}
