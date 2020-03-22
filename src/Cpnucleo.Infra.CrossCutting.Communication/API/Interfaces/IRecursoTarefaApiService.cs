using Cpnucleo.Infra.CrossCutting.Util.ViewModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cpnucleo.Infra.CrossCutting.Communication.API.Interfaces
{
    public interface IRecursoTarefaApiService : ICrudApiService<RecursoTarefaViewModel>
    {
        Task<IEnumerable<RecursoTarefaViewModel>> ListarPorTarefaAsync(string token, Guid idTarefa);
    }
}
