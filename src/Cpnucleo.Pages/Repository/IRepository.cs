using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cpnucleo.Pages.Repository
{
    public interface IRepository<T>
    {
        Task IncluirAsync(T item);

        Task<T> ConsultarAsync(int id);

        Task AlterarAsync(T item);

        Task RemoverAsync(T item);

        Task<IEnumerable<T>> ListarAsync();
    }
}
