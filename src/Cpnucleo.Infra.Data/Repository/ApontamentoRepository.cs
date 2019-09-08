using Cpnucleo.Domain.Interfaces;
using Cpnucleo.Domain.Models;
using Cpnucleo.Infra.Data.Context;
using System;
using System.Linq;

namespace Cpnucleo.Infra.Data.Repository
{
    public class ApontamentoRepository : Repository<Apontamento>, IApontamentoRepository
    {
        public ApontamentoRepository(CpnucleoContext context)
            : base(context)
        {

        }

        public void ApontarHoras(Apontamento apontamento)
        {
            throw new NotImplementedException();
        }

        public IQueryable<Apontamento> ListarPoridRecurso(Guid idRecurso)
        {
            throw new NotImplementedException();
        }

        public int ObterTotalHorasPoridRecurso(Guid idRecurso, Guid idTarefa)
        {
            throw new NotImplementedException();
        }
    }
}
