using Cpnucleo.Application.Commands.ImpedimentoTarefa.CreateImpedimentoTarefa;
using Cpnucleo.Application.Commands.ImpedimentoTarefa.RemoveImpedimentoTarefa;
using Cpnucleo.Application.Commands.ImpedimentoTarefa.UpdateImpedimentoTarefa;
using Cpnucleo.Application.Queries.ImpedimentoTarefa.GetImpedimentoTarefa;
using Cpnucleo.Application.Queries.ImpedimentoTarefa.GetImpedimentoTarefaByTarefa;
using Cpnucleo.Application.Queries.ImpedimentoTarefa.ListImpedimentoTarefa;
using MagicOnion;

namespace Cpnucleo.Application.Interfaces;

public interface IImpedimentoTarefaGrpcService : IService<IImpedimentoTarefaGrpcService>
{
    UnaryResult<OperationResult> CreateImpedimentoTarefa(CreateImpedimentoTarefaCommand command);

    UnaryResult<OperationResult> UpdateImpedimentoTarefa(UpdateImpedimentoTarefaCommand command);

    UnaryResult<GetImpedimentoTarefaViewModel> GetImpedimentoTarefa(GetImpedimentoTarefaQuery query);

    UnaryResult<ListImpedimentoTarefaViewModel> ListImpedimentoTarefa(ListImpedimentoTarefaQuery query);

    UnaryResult<OperationResult> RemoveImpedimentoTarefa(RemoveImpedimentoTarefaCommand command);

    UnaryResult<GetImpedimentoTarefaByTarefaViewModel> GetImpedimentoTarefaByTarefa(GetImpedimentoTarefaByTarefaQuery query);
}