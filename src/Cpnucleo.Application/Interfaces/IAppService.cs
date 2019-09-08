using System;
using System.Linq;

namespace Cpnucleo.Application.Interfaces
{
    public interface IAppService<TViewModel>
    {
        void Incluir(TViewModel obj);

        TViewModel Consultar(Guid id);

        IQueryable<TViewModel> Listar();

        void Alterar(TViewModel obj);

        void Remover(Guid id);
    }
}
