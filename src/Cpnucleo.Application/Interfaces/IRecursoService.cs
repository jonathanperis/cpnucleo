using Cpnucleo.Application.ViewModels;

namespace Cpnucleo.Application.Interfaces
{
    public interface IRecursoAppService : IAppService<RecursoViewModel>
    {
        new bool Incluir(RecursoViewModel recurso);

        new bool Alterar(RecursoViewModel recurso);

        RecursoViewModel ConsultarPorLogin(string login, string senha, out bool valido);
    }
}
