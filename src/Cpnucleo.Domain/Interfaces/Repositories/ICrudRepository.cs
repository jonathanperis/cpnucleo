using System;
using System.Linq;

namespace Cpnucleo.Domain.Interfaces.Repositories
{
    public interface ICrudRepository<TEntity> : IDisposable
    {
        void Incluir(TEntity obj);

        TEntity Consultar(Guid id);

        IQueryable<TEntity> Listar(bool getDependencies = false);

        void Alterar(TEntity obj);
    }
}
