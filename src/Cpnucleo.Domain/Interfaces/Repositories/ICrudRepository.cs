using System;
using System.Linq;

namespace Cpnucleo.Domain.Interfaces.Repositories
{
    public interface ICrudRepository<TModel> : IDisposable
    {
        void Incluir(TModel obj);

        IQueryable<TModel> Consultar(Guid id);

        IQueryable<TModel> Listar();

        void Alterar(TModel obj);
    }
}
