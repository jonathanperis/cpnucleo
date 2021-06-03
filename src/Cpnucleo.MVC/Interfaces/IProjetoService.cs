using Cpnucleo.Infra.CrossCutting.Util.Commands.Projeto.CreateProjeto;
using Cpnucleo.Infra.CrossCutting.Util.Commands.Projeto.RemoveProjeto;
using Cpnucleo.Infra.CrossCutting.Util.Commands.Projeto.UpdateProjeto;
using Cpnucleo.Infra.CrossCutting.Util.Queries.Projeto.GetProjeto;
using Cpnucleo.Infra.CrossCutting.Util.Queries.Projeto.ListProjeto;
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
