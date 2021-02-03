using Cpnucleo.RazorPages.ViewModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cpnucleo.RazorPages.Services.Interfaces
{
    public interface IImpedimentoTarefaApiService : ICrudApiService<ImpedimentoTarefaViewModel>
    {
        Task<IEnumerable<ImpedimentoTarefaViewModel>> ListarPorTarefaAsync(string token, Guid idTarefa);
    }
}
