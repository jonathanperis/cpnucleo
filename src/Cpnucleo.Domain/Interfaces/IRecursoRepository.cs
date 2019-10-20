using Cpnucleo.Domain.Models;
using System.Linq;

namespace Cpnucleo.Domain.Interfaces
{
    public interface IRecursoRepository : ICrudRepository<Recurso>
    {
        IQueryable<Recurso> ConsultarPorLogin(string login);
    }
}