using Cpnucleo.Infra.CrossCutting.Util.Commands.ImpedimentoTarefa.CreateImpedimentoTarefa;
using Cpnucleo.Infra.CrossCutting.Util.Commands.ImpedimentoTarefa.RemoveImpedimentoTarefa;
using Cpnucleo.Infra.CrossCutting.Util.Commands.ImpedimentoTarefa.UpdateImpedimentoTarefa;
using Cpnucleo.Infra.CrossCutting.Util.Queries.ImpedimentoTarefa.GetByTarefa;
using Cpnucleo.Infra.CrossCutting.Util.Queries.ImpedimentoTarefa.GetImpedimentoTarefa;
using Cpnucleo.Infra.CrossCutting.Util.Queries.ImpedimentoTarefa.ListImpedimentoTarefa;
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
