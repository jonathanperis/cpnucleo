using Cpnucleo.Shared.Commands.CreateRecurso;
using Cpnucleo.Shared.Commands.RemoveRecurso;
using Cpnucleo.Shared.Commands.UpdateRecurso;
using Cpnucleo.Shared.Queries.GetRecurso;
using Cpnucleo.Shared.Queries.ListRecurso;

namespace Cpnucleo.Shared.Common.Interfaces;

public interface IRecursoGrpcService : IService<IRecursoGrpcService>
{
    UnaryResult<OperationResult> CreateRecurso(CreateRecursoCommand command);

    UnaryResult<OperationResult> UpdateRecurso(UpdateRecursoCommand command);

    UnaryResult<GetRecursoViewModel> GetRecurso(GetRecursoQuery query);

    UnaryResult<ListRecursoViewModel> ListRecurso(ListRecursoQuery query);

    UnaryResult<OperationResult> RemoveRecurso(RemoveRecursoCommand command);
}