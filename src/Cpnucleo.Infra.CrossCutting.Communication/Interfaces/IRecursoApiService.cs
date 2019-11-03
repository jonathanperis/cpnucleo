using Cpnucleo.Infra.CrossCutting.Util.ViewModels;

namespace Cpnucleo.Infra.CrossCutting.Communication.Interfaces
{
    public interface IRecursoApiService : ICrudApiService<RecursoViewModel>
    {
        RecursoViewModel Autenticar(string login, string senha);
    }
}
