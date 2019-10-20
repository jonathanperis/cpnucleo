using Cpnucleo.Domain.Interfaces;
using Cpnucleo.Domain.Models;
using Cpnucleo.Infra.Data.Context;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Cpnucleo.Infra.Data.Repository
{
    public class RecursoRepository : CrudRepository<Recurso>, IRecursoRepository
    {
        public RecursoRepository(CpnucleoContext context)
            : base(context)
        {

        }

        public IQueryable<Recurso> ConsultarPorLogin(string login)
        {
            return _dbSet
                .AsNoTracking()
                .Where(x => x.Login == login && x.Ativo);
        }
    }
}
