using Cpnucleo.RazorPages.ViewModels;
using System.Threading.Tasks;

namespace Cpnucleo.RazorPages.Services.Interfaces
{
    public interface IRecursoService : ICrudService<RecursoViewModel>
    {
        Task<RecursoViewModel> AutenticarAsync(string login, string senha);
    }
}
