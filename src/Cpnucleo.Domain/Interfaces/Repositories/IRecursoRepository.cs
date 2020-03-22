using Cpnucleo.Domain.Entities;
using System.Linq;

namespace Cpnucleo.Domain.Interfaces.Repositories
{
    public interface IRecursoRepository : ICrudRepository<Recurso>
    {
        IQueryable<Recurso> ConsultarPorLogin(string login);
    }
}