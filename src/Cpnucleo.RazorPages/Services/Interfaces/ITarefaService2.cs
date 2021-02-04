using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Cpnucleo.RazorPages.ViewModels;

namespace Cpnucleo.RazorPages.Services.Interfaces
{
    public interface ITarefaService2 : ICrudService2<TarefaViewModel>
    {
        Task<(IEnumerable<TarefaViewModel> response, bool sucess, HttpStatusCode code, string message)> ListarPorRecursoAsync(string token, Guid id);

        Task<(TarefaViewModel response, bool sucess, HttpStatusCode code, string message)> AlterarPorWorkflowAsync(string token, Guid id, object value);
    }
}