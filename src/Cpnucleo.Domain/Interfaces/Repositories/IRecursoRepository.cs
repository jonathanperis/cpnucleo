using Cpnucleo.Domain.Entities;
using System.Collections.Generic;

namespace Cpnucleo.Domain.Interfaces.Repositories
{
    public interface IRecursoRepository : ICrudRepository<Recurso>
    {
        Recurso ConsultarPorLogin(string login);
    }
}