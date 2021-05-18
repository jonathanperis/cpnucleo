using Cpnucleo.Infra.CrossCutting.Util.ViewModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cpnucleo.Domain.Interfaces
{
    public interface ITarefaRepository : IGenericRepository<TarefaViewModel>
    {
        Task<IEnumerable<TarefaViewModel>> GetByRecursoAsync(Guid idRecurso);
    }
}