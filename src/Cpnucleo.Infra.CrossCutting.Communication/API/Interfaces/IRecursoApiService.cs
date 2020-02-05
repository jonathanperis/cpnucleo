using Cpnucleo.Infra.CrossCutting.Util.ViewModels;

namespace Cpnucleo.Infra.CrossCutting.Communication.API.Interfaces
{
    public interface IRecursoApiService : ICrudApiService<RecursoViewModel>
    {
        RecursoViewModel Autenticar(string login, string senha);
    }
}
