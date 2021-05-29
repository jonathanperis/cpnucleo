using Cpnucleo.Infra.CrossCutting.Util.Commands.Requests.Recurso;
using Cpnucleo.Infra.CrossCutting.Util.Commands.Responses.Recurso;
using Cpnucleo.Infra.CrossCutting.Util.Queries.Requests.Recurso;
using Cpnucleo.Infra.CrossCutting.Util.Queries.Responses.Recurso;
using System.Threading.Tasks;

namespace Cpnucleo.MVC.Interfaces
{
    public interface IRecursoService
    {
        Task<CreateRecursoResponse> AddAsync(string token, CreateRecursoCommand command);

        Task<UpdateRecursoResponse> UpdateAsync(string token, UpdateRecursoCommand command);

        Task<GetRecursoResponse> GetAsync(string token, GetRecursoQuery query);

        Task<ListRecursoResponse> AllAsync(string token, ListRecursoQuery query);

        Task<RemoveRecursoResponse> RemoveAsync(string token, RemoveRecursoCommand command);
    }
}
