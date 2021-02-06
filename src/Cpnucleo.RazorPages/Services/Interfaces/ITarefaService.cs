using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Cpnucleo.RazorPages.Models;

namespace Cpnucleo.RazorPages.Services.Interfaces
{
    public interface ITarefaService : ICrudService<TarefaViewModel>
    {
        Task<(IEnumerable<TarefaViewModel> response, bool sucess, HttpStatusCode code, string message)> ListarPorRecursoAsync(string token, Guid id);

        Task<(TarefaViewModel response, bool sucess, HttpStatusCode code, string message)> AlterarPorWorkflowAsync(string token, Guid id, object value);
    }
}