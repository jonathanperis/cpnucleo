using Cpnucleo.Shared.Commands.CreateImpedimentoTarefa;
using Cpnucleo.Shared.Commands.RemoveImpedimentoTarefa;
using Cpnucleo.Shared.Commands.UpdateImpedimentoTarefa;
using Cpnucleo.Shared.Queries.GetImpedimentoTarefa;
using Cpnucleo.Shared.Queries.ListImpedimentoTarefa;
using Cpnucleo.Shared.Queries.ListImpedimentoTarefaByTarefa;

namespace Cpnucleo.Shared.Common.Interfaces;

public interface IImpedimentoTarefaGrpcService : IService<IImpedimentoTarefaGrpcService>
{
    UnaryResult<OperationResult> CreateImpedimentoTarefa(CreateImpedimentoTarefaCommand command);

    UnaryResult<OperationResult> UpdateImpedimentoTarefa(UpdateImpedimentoTarefaCommand command);

    UnaryResult<GetImpedimentoTarefaViewModel> GetImpedimentoTarefa(GetImpedimentoTarefaQuery query);

    UnaryResult<ListImpedimentoTarefaViewModel> ListImpedimentoTarefa(ListImpedimentoTarefaQuery query);

    UnaryResult<OperationResult> RemoveImpedimentoTarefa(RemoveImpedimentoTarefaCommand command);

    UnaryResult<ListImpedimentoTarefaByTarefaViewModel> GetImpedimentoTarefaByTarefa(ListImpedimentoTarefaByTarefaQuery query);
}