using Cpnucleo.Infra.CrossCutting.Util.Commands.Requests.RecursoTarefa;
using Cpnucleo.Infra.CrossCutting.Util.Commands.Responses.RecursoTarefa;
using Cpnucleo.Infra.CrossCutting.Util.Queries.Requests.RecursoTarefa;
using Cpnucleo.Infra.CrossCutting.Util.Queries.Responses.RecursoTarefa;
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
