using Cpnucleo.Pages.Models;

namespace Cpnucleo.Pages.Repository
{
    public interface IRecursoRepository : IRepository<RecursoItem>
    {
        RecursoItem ValidarRecurso(string usuario, string senha, out bool valido);
    }
}