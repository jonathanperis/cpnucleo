using Cpnucleo.Infra.CrossCutting.Util.ViewModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cpnucleo.Application.Interfaces
{
    public interface ITarefaAppService : IGenericAppService<TarefaViewModel>
    {
        Task<IEnumerable<TarefaViewModel>> GetByRecursoAsync(Guid idRecurso);
    }
}
