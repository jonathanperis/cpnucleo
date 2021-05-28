using Cpnucleo.Infra.CrossCutting.Util.Commands.Requests.Sistema;
using Cpnucleo.Infra.CrossCutting.Util.Commands.Responses.Sistema;
using Cpnucleo.Infra.CrossCutting.Util.Queries.Requests.Sistema;
using Cpnucleo.Infra.CrossCutting.Util.Queries.Responses.Sistema;
using System.Threading.Tasks;

namespace Cpnucleo.RazorPages.Interfaces
{
    public interface ISistemaService
    {
        Task<CreateSistemaResponse> AddAsync(string token, CreateSistemaCommand command);

        Task<UpdateSistemaResponse> UpdateAsync(string token, UpdateSistemaCommand command);

        Task<GetSistemaResponse> GetAsync(string token, GetSistemaQuery query);

        Task<ListSistemaResponse> AllAsync(string token, ListSistemaQuery query);

        Task<RemoveSistemaResponse> RemoveAsync(string token, RemoveSistemaCommand command);
    }
}
