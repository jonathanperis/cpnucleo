using System;
using System.Collections.Generic;

namespace Cpnucleo.Application.Interfaces
{
    public interface IAppService<TViewModel>
    {
        bool Incluir(TViewModel obj);

        TViewModel Consultar(Guid id);

        IEnumerable<TViewModel> Listar();

        bool Alterar(TViewModel obj);

        bool Remover(Guid id);
    }
}
