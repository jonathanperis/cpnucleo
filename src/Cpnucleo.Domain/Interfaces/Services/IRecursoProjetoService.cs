using Cpnucleo.Domain.Entities;
using System;
using System.Collections.Generic;

namespace Cpnucleo.Domain.Interfaces.Services
{
    public interface IRecursoProjetoService : ICrudService<RecursoProjeto>
    {
        IEnumerable<RecursoProjeto> ListarPorProjeto(Guid idProjeto);
    }
}
