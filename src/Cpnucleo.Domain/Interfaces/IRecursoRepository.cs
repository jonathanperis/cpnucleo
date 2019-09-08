using Cpnucleo.Domain.Models;

namespace Cpnucleo.Domain.Interfaces
{
    public interface IRecursoRepository : IRepository<Recurso>
    {
        Recurso ValidarRecurso(string usuario, string senha, out bool valido);
    }
}