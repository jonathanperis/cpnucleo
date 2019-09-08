using Cpnucleo.Application.ViewModels;

namespace Cpnucleo.Application.Interfaces
{
    public interface IRecursoAppService : IAppService<RecursoViewModel>
    {
        new void Incluir(RecursoViewModel recurso);

        new void Alterar(RecursoViewModel recurso);

        RecursoViewModel Consultar(string login, string senha, out bool valido);
    }
}
