using Cpnucleo.Shared.Commands.ImpedimentoTarefa;
using Cpnucleo.Shared.Common.Models;
using Cpnucleo.Shared.Queries.ImpedimentoTarefa;

namespace Cpnucleo.Shared.Common.Interfaces;

public interface IImpedimentoTarefaGrpcService : IService<IImpedimentoTarefaGrpcService>
{
    UnaryResult<OperationResult> CreateImpedimentoTarefa(CreateImpedimentoTarefaCommand command);

    UnaryResult<OperationResult> UpdateImpedimentoTarefa(UpdateImpedimentoTarefaCommand command);

    UnaryResult<GetImpedimentoTarefaViewModel> GetImpedimentoTarefa(GetImpedimentoTarefaQuery query);

    UnaryResult<ListImpedimentoTarefaViewModel> ListImpedimentoTarefa(ListImpedimentoTarefaQuery query);

    UnaryResult<OperationResult> RemoveImpedimentoTarefa(RemoveImpedimentoTarefaCommand command);

    UnaryResult<GetImpedimentoTarefaByTarefaViewModel> GetImpedimentoTarefaByTarefa(GetImpedimentoTarefaByTarefaQuery query);
}