using Cpnucleo.Domain.Interfaces;
using Cpnucleo.Domain.Models;
using Cpnucleo.Infra.Data.Context;
using System;
using System.Linq;

namespace Cpnucleo.Infra.Data.Repository
{
    public class RecursoProjetoRepository : Repository<RecursoProjeto>, IRecursoProjetoRepository
    {
        public RecursoProjetoRepository(CpnucleoContext context)
            : base(context)
        {

        }

        public IQueryable<RecursoProjeto> ListarPoridProjeto(Guid idProjeto)
        {
            throw new NotImplementedException();
        }
    }
}
