using Cpnucleo.Application.Commands.Sistema.CreateSistema;
using Cpnucleo.Application.Commands.Sistema.RemoveSistema;
using Cpnucleo.Application.Commands.Sistema.UpdateSistema;
using Cpnucleo.Application.Queries.Sistema.GetSistema;
using Cpnucleo.Application.Queries.Sistema.ListSistema;

namespace Cpnucleo.Application.Interfaces;

public interface ISistemaGrpcService : IService<ISistemaGrpcService>
{
    UnaryResult<OperationResult> AddAsync(CreateSistemaCommand command);

    UnaryResult<OperationResult> UpdateAsync(UpdateSistemaCommand command);

    UnaryResult<GetSistemaViewModel> GetAsync(GetSistemaQuery query);

    UnaryResult<ListSistemaViewModel> AllAsync(ListSistemaQuery query);

    UnaryResult<OperationResult> RemoveAsync(RemoveSistemaCommand command);
}