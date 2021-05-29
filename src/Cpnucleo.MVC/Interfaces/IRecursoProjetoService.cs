using Cpnucleo.Infra.CrossCutting.Util.Commands.Requests.RecursoProjeto;
using Cpnucleo.Infra.CrossCutting.Util.Commands.Responses.RecursoProjeto;
using Cpnucleo.Infra.CrossCutting.Util.Queries.Requests.RecursoProjeto;
using Cpnucleo.Infra.CrossCutting.Util.Queries.Responses.RecursoProjeto;
using System.Threading.Tasks;

namespace Cpnucleo.MVC.Interfaces
{
    public interface IRecursoProjetoService
    {
        Task<CreateRecursoProjetoResponse> AddAsync(string token, CreateRecursoProjetoCommand command);

        Task<UpdateRecursoProjetoResponse> UpdateAsync(string token, UpdateRecursoProjetoCommand command);

        Task<GetRecursoProjetoResponse> GetAsync(string token, GetRecursoProjetoQuery query);

        Task<ListRecursoProjetoResponse> AllAsync(string token, ListRecursoProjetoQuery query);

        Task<RemoveRecursoProjetoResponse> RemoveAsync(string token, RemoveRecursoProjetoCommand command);

        Task<GetByProjetoResponse> GetByProjetoAsync(string token, GetByProjetoQuery query);
    }
}
