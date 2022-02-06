using Cpnucleo.Application.Commands.RecursoTarefa.CreateRecursoTarefa;
using Cpnucleo.Application.Commands.RecursoTarefa.RemoveRecursoTarefa;
using Cpnucleo.Application.Commands.RecursoTarefa.UpdateRecursoTarefa;
using Cpnucleo.Application.Queries.RecursoTarefa.GetByTarefa;
using Cpnucleo.Application.Queries.RecursoTarefa.GetRecursoTarefa;
using Cpnucleo.Application.Queries.RecursoTarefa.ListRecursoTarefa;

namespace Cpnucleo.Application.Interfaces;

public interface IRecursoTarefaGrpcService : IService<IRecursoTarefaGrpcService>
{
    UnaryResult<OperationResult> AddAsync(CreateRecursoTarefaCommand command);

    UnaryResult<OperationResult> UpdateAsync(UpdateRecursoTarefaCommand command);

    UnaryResult<GetRecursoTarefaViewModel> GetAsync(GetRecursoTarefaQuery query);

    UnaryResult<ListRecursoTarefaViewModel> AllAsync(ListRecursoTarefaQuery query);

    UnaryResult<OperationResult> RemoveAsync(RemoveRecursoTarefaCommand command);

    UnaryResult<GetRecursoTarefaByTarefaViewModel> GetByTarefaAsync(GetRecursoTarefaByTarefaQuery query);
}