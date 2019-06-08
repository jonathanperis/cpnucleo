using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cpnucleo.Pages.Repository
{
    public interface IRepository<T>
    {
        Task Incluir(T item);

        Task<T> Consultar(int id);

        Task Alterar(T item);

        Task Remover(T item);

        Task<IEnumerable<T>> Listar();
    }
}
