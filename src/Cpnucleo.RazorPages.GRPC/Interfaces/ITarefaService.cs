using Cpnucleo.Infra.CrossCutting.Util.Commands.Requests.Tarefa;
using Cpnucleo.Infra.CrossCutting.Util.Commands.Responses.Tarefa;
using Cpnucleo.Infra.CrossCutting.Util.Queries.Requests.Tarefa;
using Cpnucleo.Infra.CrossCutting.Util.Queries.Responses.Tarefa;
using System.Threading.Tasks;

namespace Cpnucleo.RazorPages.Interfaces
{
    public interface ITarefaService
    {
        Task<CreateTarefaResponse> AddAsync(string token, CreateTarefaCommand command);

        Task<UpdateTarefaResponse> UpdateAsync(string token, UpdateTarefaCommand command);

        Task<GetTarefaResponse> GetAsync(string token, GetTarefaQuery query);

        Task<ListTarefaResponse> AllAsync(string token, ListTarefaQuery query);

        Task<RemoveTarefaResponse> RemoveAsync(string token, RemoveTarefaCommand command);

        Task<GetByRecursoResponse> GetByRecursoAsync(string token, GetByRecursoQuery query);
    }
}
