using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Cpnucleo.RazorPages.ViewModels;

namespace Cpnucleo.RazorPages.Services.Interfaces
{
    public interface IImpedimentoTarefaService2 : ICrudService2<ImpedimentoTarefaViewModel>
    {
        Task<(IEnumerable<ImpedimentoTarefaViewModel> response, bool sucess, HttpStatusCode code, string message)> ListarPorTarefaAsync(string token, Guid id);
    }
}