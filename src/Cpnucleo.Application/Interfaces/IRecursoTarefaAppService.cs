using Cpnucleo.Infra.CrossCutting.Util.ViewModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cpnucleo.Domain.Interfaces
{
    public interface IRecursoTarefaRepository : IGenericRepository<RecursoTarefaViewModel>
    {
        Task<IEnumerable<RecursoTarefaViewModel>> GetByTarefaAsync(Guid idTarefa);
    }
}