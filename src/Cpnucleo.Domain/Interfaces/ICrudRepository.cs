using System;
using System.Linq;

namespace Cpnucleo.Domain.Interfaces
{
    public interface ICrudRepository<TModel> : IDisposable
    {
        void Incluir(TModel obj);

        TModel Consultar(Guid id);

        IQueryable<TModel> Listar();

        void Alterar(TModel obj);

        void Remover(Guid id);
    }
}
