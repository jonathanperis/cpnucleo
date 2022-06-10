namespace Cpnucleo.Infra.CrossCutting.Shared.Common.Interfaces;

public interface ISistemaGrpcService : IService<ISistemaGrpcService>
{
    UnaryResult<OperationResult> CreateSistema(CreateSistemaCommand command);

    UnaryResult<OperationResult> UpdateSistema(UpdateSistemaCommand command);

    UnaryResult<GetSistemaViewModel> GetSistema(GetSistemaQuery query);

    UnaryResult<ListSistemaViewModel> ListSistema(ListSistemaQuery query);

    UnaryResult<OperationResult> RemoveSistema(RemoveSistemaCommand command);
}