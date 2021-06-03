using Cpnucleo.Infra.CrossCutting.Util.Commands.RecursoTarefa.CreateRecursoTarefa;
using Cpnucleo.Infra.CrossCutting.Util.Commands.RecursoTarefa.RemoveRecursoTarefa;
using Cpnucleo.Infra.CrossCutting.Util.Commands.RecursoTarefa.UpdateRecursoTarefa;
using Cpnucleo.Infra.CrossCutting.Util.Queries.RecursoTarefa.GetByTarefa;
using Cpnucleo.Infra.CrossCutting.Util.Queries.RecursoTarefa.GetRecursoTarefa;
using Cpnucleo.Infra.CrossCutting.Util.Queries.RecursoTarefa.ListRecursoTarefa;
using System.Threading.Tasks;

namespace Cpnucleo.MVC.Interfaces
{
    public interface IRecursoTarefaService
    {
        Task<CreateRecursoTarefaResponse> AddAsync(string token, CreateRecursoTarefaCommand command);

        Task<UpdateRecursoTarefaResponse> UpdateAsync(string token, UpdateRecursoTarefaCommand command);

        Task<GetRecursoTarefaResponse> GetAsync(string token, GetRecursoTarefaQuery query);

        Task<ListRecursoTarefaResponse> AllAsync(string token, ListRecursoTarefaQuery query);

        Task<RemoveRecursoTarefaResponse> RemoveAsync(string token, RemoveRecursoTarefaCommand command);

        Task<GetByTarefaResponse> GetByTarefaAsync(string token, GetByTarefaQuery query);
    }
}
