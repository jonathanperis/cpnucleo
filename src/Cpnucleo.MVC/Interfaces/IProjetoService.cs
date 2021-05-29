using Cpnucleo.Infra.CrossCutting.Util.Commands.Requests.Projeto;
using Cpnucleo.Infra.CrossCutting.Util.Commands.Responses.Projeto;
using Cpnucleo.Infra.CrossCutting.Util.Queries.Requests.Projeto;
using Cpnucleo.Infra.CrossCutting.Util.Queries.Responses.Projeto;
using System.Threading.Tasks;

namespace Cpnucleo.MVC.Interfaces
{
    public interface IProjetoService
    {
        Task<CreateProjetoResponse> AddAsync(string token, CreateProjetoCommand command);

        Task<UpdateProjetoResponse> UpdateAsync(string token, UpdateProjetoCommand command);

        Task<GetProjetoResponse> GetAsync(string token, GetProjetoQuery query);

        Task<ListProjetoResponse> AllAsync(string token, ListProjetoQuery query);

        Task<RemoveProjetoResponse> RemoveAsync(string token, RemoveProjetoCommand command);
    }
}
