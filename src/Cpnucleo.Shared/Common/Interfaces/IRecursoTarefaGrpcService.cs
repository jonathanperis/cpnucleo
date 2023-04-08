using Cpnucleo.Shared.Commands.CreateRecursoTarefa;
using Cpnucleo.Shared.Commands.RemoveRecursoTarefa;
using Cpnucleo.Shared.Commands.UpdateRecursoTarefa;
using Cpnucleo.Shared.Queries.GetRecursoTarefa;
using Cpnucleo.Shared.Queries.ListRecursoTarefa;
using Cpnucleo.Shared.Queries.ListRecursoTarefaByTarefa;

namespace Cpnucleo.Shared.Common.Interfaces;

public interface IRecursoTarefaGrpcService : IService<IRecursoTarefaGrpcService>
{
    UnaryResult<OperationResult> CreateRecursoTarefa(CreateRecursoTarefaCommand command);

    UnaryResult<OperationResult> UpdateRecursoTarefa(UpdateRecursoTarefaCommand command);

    UnaryResult<GetRecursoTarefaViewModel> GetRecursoTarefa(GetRecursoTarefaQuery query);

    UnaryResult<ListRecursoTarefaViewModel> ListRecursoTarefa(ListRecursoTarefaQuery query);

    UnaryResult<OperationResult> RemoveRecursoTarefa(RemoveRecursoTarefaCommand command);

    UnaryResult<ListRecursoTarefaByTarefaViewModel> GetRecursoTarefaByTarefa(ListRecursoTarefaByTarefaQuery query);
}