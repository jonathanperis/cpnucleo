using Cpnucleo.RazorPages.ViewModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cpnucleo.RazorPages.Services.Interfaces
{
    public interface IApontamentoApiService : ICrudApiService<ApontamentoViewModel>
    {
        Task<IEnumerable<ApontamentoViewModel>> ListarPorRecursoAsync(string token, Guid id);
    }
}
