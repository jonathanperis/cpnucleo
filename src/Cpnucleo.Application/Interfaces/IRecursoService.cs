using Cpnucleo.Application.ViewModels;

namespace Cpnucleo.Application.Interfaces
{
    public interface IRecursoAppService : IAppService<RecursoViewModel>
    {
        RecursoViewModel ValidarRecurso(string usuario, string senha, out bool valido);
    }
}
