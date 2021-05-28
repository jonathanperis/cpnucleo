using Cpnucleo.Infra.CrossCutting.Util.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cpnucleo.RazorPages.Services.Interfaces
{
    public interface ICrudService<TViewModel>
    {
        Task<IEnumerable<TViewModel>> AllAsync(string token, string getDependencies, SistemaViewModel viewModel);
    }
}
