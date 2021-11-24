using Cpnucleo.Infra.CrossCutting.Util.Commands.Sistema;
using Cpnucleo.Infra.CrossCutting.Util.Queries.Sistema;

namespace Cpnucleo.Infra.CrossCutting.Util.Interfaces;

public interface ISistemaGrpcService : IService<ISistemaGrpcService>
{
    UnaryResult<OperationResult> AddAsync(CreateSistemaCommand command);

    UnaryResult<OperationResult> UpdateAsync(UpdateSistemaCommand command);

    UnaryResult<SistemaViewModel> GetAsync(GetSistemaQuery query);

    UnaryResult<IEnumerable<SistemaViewModel>> AllAsync(ListSistemaQuery query);

    UnaryResult<OperationResult> RemoveAsync(RemoveSistemaCommand command);
}