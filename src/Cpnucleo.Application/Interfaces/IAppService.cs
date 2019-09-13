using System;
using System.Linq;

namespace Cpnucleo.Application.Interfaces
{
    public interface IAppService<TViewModel>
    {
        bool Incluir(TViewModel obj);

        TViewModel Consultar(Guid id);

        IQueryable<TViewModel> Listar();

        bool Alterar(TViewModel obj);

        bool Remover(Guid id);
    }
}
