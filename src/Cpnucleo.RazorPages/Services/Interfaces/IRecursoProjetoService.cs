using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Cpnucleo.RazorPages.Models;

namespace Cpnucleo.RazorPages.Services.Interfaces
{
    public interface IRecursoProjetoService : ICrudService<RecursoProjetoViewModel>
    {
        Task<(IEnumerable<RecursoProjetoViewModel> response, bool sucess, HttpStatusCode code, string message)> ListarPorProjetoAsync(string token, Guid id);
    }
}