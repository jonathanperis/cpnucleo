using Cpnucleo.Infra.CrossCutting.Util.Commands.Sistema.CreateSistema;
using Cpnucleo.Infra.CrossCutting.Util.Commands.Sistema.RemoveSistema;
using Cpnucleo.Infra.CrossCutting.Util.Commands.Sistema.UpdateSistema;
using Cpnucleo.Infra.CrossCutting.Util.Queries.Sistema.GetSistema;
using Cpnucleo.Infra.CrossCutting.Util.Queries.Sistema.ListSistema;
using MagicOnion;

namespace Cpnucleo.Infra.CrossCutting.Util.Interfaces
{
    public interface ISistemaGrpcService : IService<ISistemaGrpcService>
    {
        UnaryResult<CreateSistemaResponse> AddAsync(CreateSistemaCommand command);

        UnaryResult<UpdateSistemaResponse> UpdateAsync(UpdateSistemaCommand command);

        UnaryResult<GetSistemaResponse> GetAsync(GetSistemaQuery query);

        UnaryResult<ListSistemaResponse> AllAsync(ListSistemaQuery query);

        UnaryResult<RemoveSistemaResponse> RemoveAsync(RemoveSistemaCommand command);
    }
}
