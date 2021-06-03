using Cpnucleo.Infra.CrossCutting.Util.Commands.Sistema.CreateSistema;
using Cpnucleo.Infra.CrossCutting.Util.Commands.Sistema.RemoveSistema;
using Cpnucleo.Infra.CrossCutting.Util.Commands.Sistema.UpdateSistema;
using Cpnucleo.Infra.CrossCutting.Util.Queries.Sistema.GetSistema;
using Cpnucleo.Infra.CrossCutting.Util.Queries.Sistema.ListSistema;
using System.Threading.Tasks;

namespace Cpnucleo.MVC.Interfaces
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
