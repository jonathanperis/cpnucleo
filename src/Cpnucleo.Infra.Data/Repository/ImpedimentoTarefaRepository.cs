using Cpnucleo.Domain.Entities;
using Cpnucleo.Domain.Interfaces.Repositories;
using Cpnucleo.Infra.Data.Context;
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

        public IQueryable<ImpedimentoTarefa> ListarPorTarefa(Guid idTarefa)
        {
            return Listar()
                .Where(x => x.IdTarefa == idTarefa);
        }
    }
}
