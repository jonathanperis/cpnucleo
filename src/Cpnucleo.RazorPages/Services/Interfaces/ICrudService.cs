using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cpnucleo.RazorPages.Services.Interfaces
{
    public interface ICrudService<TViewModel>
    {
        Task<bool> IncluirAsync(string token, TViewModel obj);

        Task<TViewModel> ConsultarAsync(string token, Guid id);

        Task<IEnumerable<TViewModel>> ListarAsync(string token, bool getDependencies = false);

        Task<bool> AlterarAsync(string token, TViewModel obj);

        Task<bool> RemoverAsync(string token, Guid id);
    }
}
