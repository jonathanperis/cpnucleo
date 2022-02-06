using Cpnucleo.Application.Commands.RecursoTarefa.CreateRecursoTarefa;
using Cpnucleo.Application.Commands.RecursoTarefa.RemoveRecursoTarefa;
using Cpnucleo.Application.Commands.RecursoTarefa.UpdateRecursoTarefa;
using Cpnucleo.Application.Queries.RecursoTarefa.GetRecursoTarefa;
using Cpnucleo.Application.Queries.RecursoTarefa.GetRecursoTarefaByTarefa;
using Cpnucleo.Application.Queries.RecursoTarefa.ListRecursoTarefa;
using MagicOnion;

namespace Cpnucleo.Application.Common.Interfaces;

public interface IRecursoTarefaGrpcService : IService<IRecursoTarefaGrpcService>
{
    UnaryResult<OperationResult> CreateRecursoTarefa(CreateRecursoTarefaCommand command);

    UnaryResult<OperationResult> UpdateRecursoTarefa(UpdateRecursoTarefaCommand command);

    UnaryResult<GetRecursoTarefaViewModel> GetRecursoTarefa(GetRecursoTarefaQuery query);

    UnaryResult<ListRecursoTarefaViewModel> ListRecursoTarefa(ListRecursoTarefaQuery query);

    UnaryResult<OperationResult> RemoveRecursoTarefa(RemoveRecursoTarefaCommand command);

    UnaryResult<GetRecursoTarefaByTarefaViewModel> GetRecursoTarefaByTarefa(GetRecursoTarefaByTarefaQuery query);
}