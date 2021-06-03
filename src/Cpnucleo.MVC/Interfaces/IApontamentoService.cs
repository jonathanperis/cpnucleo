using Cpnucleo.Infra.CrossCutting.Util.Commands.Apontamento.CreateApontamento;
using Cpnucleo.Infra.CrossCutting.Util.Commands.Apontamento.RemoveApontamento;
using Cpnucleo.Infra.CrossCutting.Util.Commands.Apontamento.UpdateApontamento;
using Cpnucleo.Infra.CrossCutting.Util.Queries.Apontamento.GetApontamento;
using Cpnucleo.Infra.CrossCutting.Util.Queries.Apontamento.GetByRecurso;
using Cpnucleo.Infra.CrossCutting.Util.Queries.Apontamento.GetTotalHorasPorRecurso;
using Cpnucleo.Infra.CrossCutting.Util.Queries.Apontamento.ListApontamento;
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
