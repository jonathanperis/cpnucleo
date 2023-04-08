using Cpnucleo.Shared.Commands.CreateSistema;
using Cpnucleo.Shared.Commands.RemoveSistema;
using Cpnucleo.Shared.Commands.UpdateSistema;
using Cpnucleo.Shared.Queries.GetSistema;
using Cpnucleo.Shared.Queries.ListSistema;

namespace Cpnucleo.Shared.Common.Interfaces;

public interface ISistemaGrpcService : IService<ISistemaGrpcService>
{
    UnaryResult<OperationResult> CreateSistema(CreateSistemaCommand command);

    UnaryResult<OperationResult> UpdateSistema(UpdateSistemaCommand command);

    UnaryResult<GetSistemaViewModel> GetSistema(GetSistemaQuery query);

    UnaryResult<ListSistemaViewModel> ListSistema(ListSistemaQuery query);

    UnaryResult<OperationResult> RemoveSistema(RemoveSistemaCommand command);
}