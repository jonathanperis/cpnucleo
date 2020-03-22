using Cpnucleo.Infra.CrossCutting.Util.ViewModels;
using System.Threading.Tasks;

namespace Cpnucleo.Infra.CrossCutting.Communication.API.Interfaces
{
    public interface IRecursoApiService : ICrudApiService<RecursoViewModel>
    {
        Task<RecursoViewModel> AutenticarAsync(string login, string senha);
    }
}
