using Cpnucleo.Domain.Interfaces;
using Cpnucleo.Domain.Models;
using Cpnucleo.Infra.Data.Context;
using System;
using System.Linq;

namespace Cpnucleo.Infra.Data.Repository
{
    public class RecursoProjetoRepository : CrudRepository<RecursoProjeto>, IRecursoProjetoRepository
    {
        public RecursoProjetoRepository(CpnucleoContext context)
            : base(context)
        {

        }

        public IQueryable<RecursoProjeto> ListarPorProjeto(Guid idProjeto)
        {
            return Listar()
                .Where(x => x.IdProjeto == idProjeto);
        }
    }
}
