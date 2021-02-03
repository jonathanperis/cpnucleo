using Cpnucleo.RazorPages.ViewModels;
using System.Threading.Tasks;

namespace Cpnucleo.RazorPages.Services.Interfaces
{
    public interface IRecursoApiService : ICrudApiService<RecursoViewModel>
    {
        Task<RecursoViewModel> AutenticarAsync(string login, string senha);
    }
}
