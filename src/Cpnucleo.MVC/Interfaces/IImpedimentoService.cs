using Cpnucleo.Infra.CrossCutting.Util.Commands.Impedimento.CreateImpedimento;
using Cpnucleo.Infra.CrossCutting.Util.Commands.Impedimento.RemoveImpedimento;
using Cpnucleo.Infra.CrossCutting.Util.Commands.Impedimento.UpdateImpedimento;
using Cpnucleo.Infra.CrossCutting.Util.Queries.Impedimento.GetImpedimento;
using Cpnucleo.Infra.CrossCutting.Util.Queries.Impedimento.ListImpedimento;
using System.Threading.Tasks;

namespace Cpnucleo.MVC.Interfaces
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
