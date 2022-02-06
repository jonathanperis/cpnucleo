using Cpnucleo.Application.Commands.Tarefa.CreateTarefa;
using Cpnucleo.Application.Commands.Tarefa.RemoveTarefa;
using Cpnucleo.Application.Commands.Tarefa.UpdateTarefa;
using Cpnucleo.Application.Queries.Tarefa.GetTarefa;
using Cpnucleo.Application.Queries.Tarefa.GetTarefaByRecurso;
using Cpnucleo.Application.Queries.Tarefa.ListTarefa;
using MagicOnion;

namespace Cpnucleo.Application.Common.Interfaces;

public interface ITarefaGrpcService : IService<ITarefaGrpcService>
{
    UnaryResult<OperationResult> CreateTarefa(CreateTarefaCommand command);

    UnaryResult<OperationResult> UpdateTarefa(UpdateTarefaCommand command);

    UnaryResult<GetTarefaViewModel> GetTarefa(GetTarefaQuery query);

    UnaryResult<ListTarefaViewModel> ListTarefa(ListTarefaQuery query);

    UnaryResult<OperationResult> RemoveTarefa(RemoveTarefaCommand command);

    UnaryResult<GetTarefaByRecursoViewModel> GetTarefaByRecurso(GetTarefaByRecursoQuery query);
}