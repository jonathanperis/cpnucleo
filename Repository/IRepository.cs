using System.Collections.Generic;
using System.Threading.Tasks;

namespace dotnet_cpnucleo_pages.Repository
{
    public interface IRepository<T>
    {
        Task Incluir(T item);

        Task<T> Consultar(int id);

        Task Alterar(T item);

        Task Remover(T item);

        Task<IList<T>> Listar();
    }
}
