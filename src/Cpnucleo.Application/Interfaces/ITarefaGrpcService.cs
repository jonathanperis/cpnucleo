using Cpnucleo.Application.Commands.Tarefa.CreateTarefa;
using Cpnucleo.Application.Commands.Tarefa.RemoveTarefa;
using Cpnucleo.Application.Commands.Tarefa.UpdateTarefa;
using Cpnucleo.Application.Queries.Tarefa.GetTarefa;
using Cpnucleo.Application.Queries.Tarefa.GetTarefaByRecurso;
using Cpnucleo.Application.Queries.Tarefa.ListTarefa;

namespace Cpnucleo.Application.Interfaces;

public interface ITarefaGrpcService : IService<ITarefaGrpcService>
{
    UnaryResult<OperationResult> AddAsync(CreateTarefaCommand command);

    UnaryResult<OperationResult> UpdateAsync(UpdateTarefaCommand command);

    UnaryResult<GetTarefaViewModel> GetAsync(GetTarefaQuery query);

    UnaryResult<ListTarefaViewModel> AllAsync(ListTarefaQuery query);

    UnaryResult<OperationResult> RemoveAsync(RemoveTarefaCommand command);

    UnaryResult<GetTarefaByRecursoViewModel> GetByRecursoAsync(GetTarefaByRecursoQuery query);
}