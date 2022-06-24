using Cpnucleo.Shared.Commands.Recurso;
using Cpnucleo.Shared.Common.Models;
using Cpnucleo.Shared.Queries.Recurso;

namespace Cpnucleo.Shared.Common.Interfaces;

public interface IRecursoGrpcService : IService<IRecursoGrpcService>
{
    UnaryResult<OperationResult> CreateRecurso(CreateRecursoCommand command);

    UnaryResult<OperationResult> UpdateRecurso(UpdateRecursoCommand command);

    UnaryResult<GetRecursoViewModel> GetRecurso(GetRecursoQuery query);

    UnaryResult<ListRecursoViewModel> ListRecurso(ListRecursoQuery query);

    UnaryResult<OperationResult> RemoveRecurso(RemoveRecursoCommand command);
}