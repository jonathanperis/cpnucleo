using Cpnucleo.Infra.CrossCutting.Util.Commands.Requests.Impedimento;
using Cpnucleo.Infra.CrossCutting.Util.Commands.Responses.Impedimento;
using Cpnucleo.Infra.CrossCutting.Util.Queries.Requests.Impedimento;
using Cpnucleo.Infra.CrossCutting.Util.Queries.Responses.Impedimento;
using System.Threading.Tasks;

namespace Cpnucleo.RazorPages.Interfaces
{
    public interface IImpedimentoService
    {
        Task<CreateImpedimentoResponse> AddAsync(string token, CreateImpedimentoCommand command);

        Task<UpdateImpedimentoResponse> UpdateAsync(string token, UpdateImpedimentoCommand command);

        Task<GetImpedimentoResponse> GetAsync(string token, GetImpedimentoQuery query);

        Task<ListImpedimentoResponse> AllAsync(string token, ListImpedimentoQuery query);

        Task<RemoveImpedimentoResponse> RemoveAsync(string token, RemoveImpedimentoCommand command);
    }
}
