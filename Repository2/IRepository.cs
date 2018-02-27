using System.Collections.Generic;
using System.Threading.Tasks;

namespace dotnet_cpnucleo_pages.Repository2
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
