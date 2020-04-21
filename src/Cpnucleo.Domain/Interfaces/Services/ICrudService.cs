using System;
using System.Collections.Generic;

namespace Cpnucleo.Domain.Interfaces.Services
{
    public interface ICrudService<TEntity>
    {
        Guid Incluir(TEntity obj);

        TEntity Consultar(Guid id);

        IEnumerable<TEntity> Listar(bool getDependencies = false);

        bool Alterar(TEntity obj);

        bool Remover(Guid id);
    }
}
