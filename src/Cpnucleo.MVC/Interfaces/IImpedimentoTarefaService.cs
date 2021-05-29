using Cpnucleo.Infra.CrossCutting.Util.Commands.Requests.ImpedimentoTarefa;
using Cpnucleo.Infra.CrossCutting.Util.Commands.Responses.ImpedimentoTarefa;
using Cpnucleo.Infra.CrossCutting.Util.Queries.Requests.ImpedimentoTarefa;
using Cpnucleo.Infra.CrossCutting.Util.Queries.Responses.ImpedimentoTarefa;
using System.Threading.Tasks;

namespace Cpnucleo.MVC.Interfaces
{
    public interface IImpedimentoTarefaService
    {
        Task<CreateImpedimentoTarefaResponse> AddAsync(string token, CreateImpedimentoTarefaCommand command);

        Task<UpdateImpedimentoTarefaResponse> UpdateAsync(string token, UpdateImpedimentoTarefaCommand command);

        Task<GetImpedimentoTarefaResponse> GetAsync(string token, GetImpedimentoTarefaQuery query);

        Task<ListImpedimentoTarefaResponse> AllAsync(string token, ListImpedimentoTarefaQuery query);

        Task<RemoveImpedimentoTarefaResponse> RemoveAsync(string token, RemoveImpedimentoTarefaCommand command);

        Task<GetByTarefaResponse> GetByTarefaAsync(string token, GetByTarefaQuery query);
    }
}
