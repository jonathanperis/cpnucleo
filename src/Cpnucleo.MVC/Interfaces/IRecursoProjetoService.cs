using Cpnucleo.Infra.CrossCutting.Util.Commands.RecursoProjeto.CreateRecursoProjeto;
using Cpnucleo.Infra.CrossCutting.Util.Commands.RecursoProjeto.RemoveRecursoProjeto;
using Cpnucleo.Infra.CrossCutting.Util.Commands.RecursoProjeto.UpdateRecursoProjeto;
using Cpnucleo.Infra.CrossCutting.Util.Queries.RecursoProjeto.GetByProjeto;
using Cpnucleo.Infra.CrossCutting.Util.Queries.RecursoProjeto.GetRecursoProjeto;
using Cpnucleo.Infra.CrossCutting.Util.Queries.RecursoProjeto.ListRecursoProjeto;
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
