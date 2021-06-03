using Cpnucleo.Infra.CrossCutting.Util.Commands.Recurso.CreateRecurso;
using Cpnucleo.Infra.CrossCutting.Util.Commands.Recurso.RemoveRecurso;
using Cpnucleo.Infra.CrossCutting.Util.Commands.Recurso.UpdateRecurso;
using Cpnucleo.Infra.CrossCutting.Util.Queries.Recurso.Auth;
using Cpnucleo.Infra.CrossCutting.Util.Queries.Recurso.GetRecurso;
using Cpnucleo.Infra.CrossCutting.Util.Queries.Recurso.ListRecurso;
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

        Task<AuthResponse> AuthAsync(AuthQuery query);
    }
}
