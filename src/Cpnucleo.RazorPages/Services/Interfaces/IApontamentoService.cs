using Cpnucleo.RazorPages.ViewModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cpnucleo.RazorPages.Services.Interfaces
{
    public interface IApontamentoService : ICrudService<ApontamentoViewModel>
    {
        Task<IEnumerable<ApontamentoViewModel>> ListarPorRecursoAsync(string token, Guid id);
    }
}
