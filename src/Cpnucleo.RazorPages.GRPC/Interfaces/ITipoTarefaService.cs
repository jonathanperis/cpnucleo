using Cpnucleo.Infra.CrossCutting.Util.Commands.Requests.TipoTarefa;
using Cpnucleo.Infra.CrossCutting.Util.Commands.Responses.TipoTarefa;
using Cpnucleo.Infra.CrossCutting.Util.Queries.Requests.TipoTarefa;
using Cpnucleo.Infra.CrossCutting.Util.Queries.Responses.TipoTarefa;
using System.Threading.Tasks;

namespace Cpnucleo.RazorPages.Interfaces
{
    public interface ITipoTarefaService
    {
        Task<CreateTipoTarefaResponse> AddAsync(string token, CreateTipoTarefaCommand command);

        Task<UpdateTipoTarefaResponse> UpdateAsync(string token, UpdateTipoTarefaCommand command);

        Task<GetTipoTarefaResponse> GetAsync(string token, GetTipoTarefaQuery query);

        Task<ListTipoTarefaResponse> AllAsync(string token, ListTipoTarefaQuery query);

        Task<RemoveTipoTarefaResponse> RemoveAsync(string token, RemoveTipoTarefaCommand command);
    }
}
