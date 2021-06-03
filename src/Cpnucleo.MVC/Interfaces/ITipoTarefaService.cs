using Cpnucleo.Infra.CrossCutting.Util.Commands.TipoTarefa.CreateTipoTarefa;
using Cpnucleo.Infra.CrossCutting.Util.Commands.TipoTarefa.RemoveTipoTarefa;
using Cpnucleo.Infra.CrossCutting.Util.Commands.TipoTarefa.UpdateTipoTarefa;
using Cpnucleo.Infra.CrossCutting.Util.Queries.TipoTarefa.GetTipoTarefa;
using Cpnucleo.Infra.CrossCutting.Util.Queries.TipoTarefa.ListTipoTarefa;
using System.Threading.Tasks;

namespace Cpnucleo.MVC.Interfaces
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
