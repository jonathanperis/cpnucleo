using Cpnucleo.Shared.Commands.CreateTarefa;
using Cpnucleo.Shared.Commands.RemoveTarefa;
using Cpnucleo.Shared.Commands.UpdateTarefa;
using Cpnucleo.Shared.Commands.UpdateTarefaByWorkflow;
using Cpnucleo.Shared.Queries.GetTarefa;
using Cpnucleo.Shared.Queries.ListTarefa;
using Cpnucleo.Shared.Queries.ListTarefaByRecurso;

namespace Cpnucleo.Shared.Common.Interfaces;

public interface ITarefaGrpcService : IService<ITarefaGrpcService>
{
    UnaryResult<OperationResult> CreateTarefa(CreateTarefaCommand command);

    UnaryResult<OperationResult> UpdateTarefa(UpdateTarefaCommand command);

    UnaryResult<GetTarefaViewModel> GetTarefa(GetTarefaQuery query);

    UnaryResult<ListTarefaViewModel> ListTarefa(ListTarefaQuery query);

    UnaryResult<OperationResult> RemoveTarefa(RemoveTarefaCommand command);

    UnaryResult<ListTarefaByRecursoViewModel> GetTarefaByRecurso(ListTarefaByRecursoQuery query);

    UnaryResult<OperationResult> UpdateTarefaByWorkflow(UpdateTarefaByWorkflowCommand command);
}