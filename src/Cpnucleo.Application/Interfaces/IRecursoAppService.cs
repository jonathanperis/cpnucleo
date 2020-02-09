using Cpnucleo.Infra.CrossCutting.Util.ViewModels;

namespace Cpnucleo.Application.Interfaces
{
    public interface IRecursoAppService : ICrudAppService<RecursoViewModel>
    {
        RecursoViewModel Autenticar(string login, string senha, out bool valido);
    }
}
