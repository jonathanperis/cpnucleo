using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Cpnucleo.RazorPages.ViewModels;

namespace Cpnucleo.RazorPages.Services.Interfaces
{
    public interface IRecursoProjetoService2 : ICrudService2<RecursoProjetoViewModel>
    {
        Task<(IEnumerable<RecursoProjetoViewModel> response, bool sucess, HttpStatusCode code, string message)> ListarPorProjetoAsync(string token, Guid id);
    }
}