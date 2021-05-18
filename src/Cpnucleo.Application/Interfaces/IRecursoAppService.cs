using Cpnucleo.Infra.CrossCutting.Util.ViewModels;
using System.Threading.Tasks;

namespace Cpnucleo.Domain.Interfaces
{
    public interface IRecursoRepository : IGenericRepository<RecursoViewModel>
    {
        Task<RecursoViewModel> GetByLoginAsync(string login);
    }
}