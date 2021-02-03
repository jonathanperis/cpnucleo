using Cpnucleo.RazorPages.ViewModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cpnucleo.RazorPages.Services.Interfaces
{
    public interface IImpedimentoTarefaService : ICrudService<ImpedimentoTarefaViewModel>
    {
        Task<IEnumerable<ImpedimentoTarefaViewModel>> ListarPorTarefaAsync(string token, Guid idTarefa);
    }
}
