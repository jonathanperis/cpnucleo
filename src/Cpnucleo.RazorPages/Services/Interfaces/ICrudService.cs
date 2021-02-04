using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace Cpnucleo.RazorPages.Services.Interfaces
{
    public interface ICrudService<TViewModel>
    {
        Task<(IEnumerable<TViewModel> response, bool sucess, HttpStatusCode code, string message)> ListarAsync(string token, bool getDependencies = false);

        Task<(TViewModel response, bool sucess, HttpStatusCode code, string message)> ConsultarAsync(string token, Guid id);

        Task<(TViewModel response, bool sucess, HttpStatusCode code, string message)> IncluirAsync(string token, object value);

        Task<(TViewModel response, bool sucess, HttpStatusCode code, string message)> AlterarAsync(string token, Guid id, object value);

        Task<(TViewModel response, bool sucess, HttpStatusCode code, string message)> RemoverAsync(string token, Guid id);        
    }
}
