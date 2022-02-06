using Cpnucleo.Application.Commands.Sistema.CreateSistema;
using Cpnucleo.Application.Commands.Sistema.RemoveSistema;
using Cpnucleo.Application.Commands.Sistema.UpdateSistema;
using Cpnucleo.Application.Queries.Sistema.GetSistema;
using Cpnucleo.Application.Queries.Sistema.ListSistema;
using MagicOnion;

namespace Cpnucleo.Application.Interfaces;

public interface ISistemaGrpcService : IService<ISistemaGrpcService>
{
    UnaryResult<OperationResult> CreateSistema(CreateSistemaCommand command);

    UnaryResult<OperationResult> UpdateSistema(UpdateSistemaCommand command);

    UnaryResult<GetSistemaViewModel> GetSistema(GetSistemaQuery query);

    UnaryResult<ListSistemaViewModel> ListSistema(ListSistemaQuery query);

    UnaryResult<OperationResult> RemoveSistema(RemoveSistemaCommand command);
}