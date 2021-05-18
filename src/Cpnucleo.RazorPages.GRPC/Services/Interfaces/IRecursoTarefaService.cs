using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Cpnucleo.RazorPages.Models;

namespace Cpnucleo.RazorPages.Services.Interfaces
{
    public interface IRecursoTarefaService : ICrudService<RecursoTarefaViewModel>
    {
        Task<(IEnumerable<RecursoTarefaViewModel> response, bool sucess, HttpStatusCode code, string message)> ListarPorTarefaAsync(string token, Guid id);
    }
}