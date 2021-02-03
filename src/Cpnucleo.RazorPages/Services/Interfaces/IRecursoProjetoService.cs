using Cpnucleo.RazorPages.ViewModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cpnucleo.RazorPages.Services.Interfaces
{
    public interface IRecursoProjetoService : ICrudService<RecursoProjetoViewModel>
    {
        Task<IEnumerable<RecursoProjetoViewModel>> ListarPorProjetoAsync(string token, Guid idProjeto);
    }
}
