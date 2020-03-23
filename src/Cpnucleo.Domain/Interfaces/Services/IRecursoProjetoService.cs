using Cpnucleo.Domain.Entities;
using System;
using System.Linq;

namespace Cpnucleo.Domain.Interfaces.Services
{
    public interface IRecursoProjetoService : ICrudService<RecursoProjeto>
    {
        IQueryable<RecursoProjeto> ListarPorProjeto(Guid idProjeto);
    }
}
