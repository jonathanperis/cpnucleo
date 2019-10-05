using Cpnucleo.Domain.Models;

namespace Cpnucleo.Domain.Interfaces
{
    public interface IRecursoRepository : ICrudRepository<Recurso>
    {
        Recurso ConsultarPorLogin(string login);
    }
}