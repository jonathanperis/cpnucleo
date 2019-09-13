using Cpnucleo.Domain.Interfaces;
using Cpnucleo.Domain.Models;
using Cpnucleo.Infra.Data.Context;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Cpnucleo.Infra.Data.Repository
{
    public class RecursoRepository : Repository<Recurso>, IRecursoRepository
    {
        public RecursoRepository(CpnucleoContext context)
            : base(context)
        {

        }

        public Recurso ValidarRecurso(string login)
        {
            return DbSet
                .AsNoTracking()
                .FirstOrDefault(x => x.Login == login);
        }
    }
}
