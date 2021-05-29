using Cpnucleo.Infra.CrossCutting.Util.Commands.Requests.Apontamento;
using Cpnucleo.Infra.CrossCutting.Util.Commands.Responses.Apontamento;
using Cpnucleo.Infra.CrossCutting.Util.Queries.Requests.Apontamento;
using Cpnucleo.Infra.CrossCutting.Util.Queries.Responses.Apontamento;
using System.Threading.Tasks;

namespace Cpnucleo.MVC.Interfaces
{
    public interface IApontamentoService
    {
        Task<CreateApontamentoResponse> AddAsync(string token, CreateApontamentoCommand command);

        Task<UpdateApontamentoResponse> UpdateAsync(string token, UpdateApontamentoCommand command);

        Task<GetApontamentoResponse> GetAsync(string token, GetApontamentoQuery query);

        Task<ListApontamentoResponse> AllAsync(string token, ListApontamentoQuery query);

        Task<RemoveApontamentoResponse> RemoveAsync(string token, RemoveApontamentoCommand command);

        Task<GetByRecursoResponse> GetByRecursoAsync(string token, GetByRecursoQuery query);

        Task<GetTotalHorasPorRecursoResponse> GetTotalHorasPorRecursoAsync(string token, GetTotalHorasPorRecursoQuery query);
    }
}
