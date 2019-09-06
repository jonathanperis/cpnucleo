using Cpnucleo.Pages.Models;

namespace Cpnucleo.Pages.Repository
{
    public interface IRecursoRepository : IRepository<RecursoModel>
    {
        RecursoModel ValidarRecurso(string usuario, string senha, out bool valido);
    }
}