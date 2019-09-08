using Cpnucleo.Domain.Models;

namespace Cpnucleo.Domain.Interfaces
{
    public interface IRecursoRepository : IRepository<Recurso>
    {
        Recurso ValidarRecurso(string login);
    }
}