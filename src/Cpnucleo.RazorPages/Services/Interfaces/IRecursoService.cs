using System.Net;
using System.Threading.Tasks;
using Cpnucleo.RazorPages.ViewModels;

namespace Cpnucleo.RazorPages.Services.Interfaces
{
    public interface IRecursoService : ICrudService<RecursoViewModel>
    {
        Task<(RecursoViewModel response, bool sucess, HttpStatusCode code, string message)> AutenticarAsync(string username, string password);
    }
}