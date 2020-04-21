using System;
using System.Collections.Generic;

namespace Cpnucleo.Application.Interfaces
{
    public interface ICrudAppService<TViewModel>
    {
        Guid Incluir(TViewModel obj);

        TViewModel Consultar(Guid id);

        IEnumerable<TViewModel> Listar(bool getDependencies = false);

        bool Alterar(TViewModel obj);

        bool Remover(Guid id);
    }
}
