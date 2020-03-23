using System;
using System.Linq;

namespace Cpnucleo.Domain.Interfaces.Services
{
    public interface ICrudService<TModel>
    {
        bool Incluir(TModel obj);

        IQueryable<TModel> Consultar(Guid id);

        IQueryable<TModel> Listar();

        bool Alterar(TModel obj);

        bool Remover(Guid id);
    }
}
