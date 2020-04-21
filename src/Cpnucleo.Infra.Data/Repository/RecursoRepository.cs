using Cpnucleo.Domain.Entities;
using Cpnucleo.Domain.Interfaces.Repositories;
using Cpnucleo.Infra.Data.Context;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Cpnucleo.Infra.Data.Repository
{
    internal class RecursoRepository : CrudRepository<Recurso>, IRecursoRepository
    {
        public RecursoRepository(CpnucleoContext context)
            : base(context)
        {

        }

        public Recurso ConsultarPorLogin(string login)
        {
            return _dbSet
                .AsNoTracking()
                .FirstOrDefault(x => x.Login == login && x.Ativo);
        }
    }
}
