using Cpnucleo.Application.ViewModels;

namespace Cpnucleo.Application.Interfaces
{
    public interface IRecursoAppService : ICrudAppService<RecursoViewModel>
    {
        new bool Incluir(RecursoViewModel recurso);

        new bool Alterar(RecursoViewModel recurso);

        RecursoViewModel Autenticar(string login, string senha, out bool valido);
    }
}
