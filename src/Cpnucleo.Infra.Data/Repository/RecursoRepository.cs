using Cpnucleo.Domain.Interfaces;
using Cpnucleo.Domain.Models;
using Cpnucleo.Infra.Data.Context;
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
            return DbSet.FirstOrDefault(x => x.Login == login);
        }
    }
}
