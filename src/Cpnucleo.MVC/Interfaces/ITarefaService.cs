using Cpnucleo.Infra.CrossCutting.Util.Commands.Tarefa.CreateTarefa;
using Cpnucleo.Infra.CrossCutting.Util.Commands.Tarefa.RemoveTarefa;
using Cpnucleo.Infra.CrossCutting.Util.Commands.Tarefa.UpdateTarefa;
using Cpnucleo.Infra.CrossCutting.Util.Queries.Tarefa.GetByRecurso;
using Cpnucleo.Infra.CrossCutting.Util.Queries.Tarefa.GetTarefa;
using Cpnucleo.Infra.CrossCutting.Util.Queries.Tarefa.ListTarefa;
using System.Threading.Tasks;

namespace Cpnucleo.MVC.Interfaces
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
