using Cpnucleo.Infra.CrossCutting.Util.Commands.Recurso;
using Cpnucleo.Infra.CrossCutting.Util.Queries.Recurso.Auth;
using Cpnucleo.Infra.CrossCutting.Util.Queries.Recurso;

namespace Cpnucleo.Infra.CrossCutting.Util.Interfaces;

public interface IRecursoGrpcService : IService<IRecursoGrpcService>
{
    UnaryResult<OperationResult> AddAsync(CreateRecursoCommand command);

    UnaryResult<OperationResult> UpdateAsync(UpdateRecursoCommand command);

    UnaryResult<RecursoViewModel> GetAsync(GetRecursoQuery query);

    UnaryResult<IEnumerable<RecursoViewModel>> AllAsync(ListRecursoQuery query);

    UnaryResult<OperationResult> RemoveAsync(RemoveRecursoCommand command);

    UnaryResult<AuthResponse> AuthAsync(AuthQuery query);
}