using Cpnucleo.Domain.Interfaces;
using Cpnucleo.Domain.Models;
using Cpnucleo.Infra.Data.Context;
using System;
using System.Linq;

namespace Cpnucleo.Infra.Data.Repository
{
    public class ImpedimentoTarefaRepository : Repository<ImpedimentoTarefa>, IImpedimentoTarefaRepository
    {
        public ImpedimentoTarefaRepository(CpnucleoContext context)
            : base(context)
        {

        }

        public IQueryable<ImpedimentoTarefa> ListarPoridTarefa(Guid idTarefa)
        {
            throw new NotImplementedException();
        }
    }
}
