using Cpnucleo.Application.Commands.ImpedimentoTarefa.CreateImpedimentoTarefa;
using Cpnucleo.Application.Commands.ImpedimentoTarefa.RemoveImpedimentoTarefa;
using Cpnucleo.Application.Commands.ImpedimentoTarefa.UpdateImpedimentoTarefa;
using Cpnucleo.Application.Queries.ImpedimentoTarefa.GetByTarefa;
using Cpnucleo.Application.Queries.ImpedimentoTarefa.GetImpedimentoTarefa;
using Cpnucleo.Application.Queries.ImpedimentoTarefa.ListImpedimentoTarefa;

namespace Cpnucleo.Application.Interfaces;

public interface IImpedimentoTarefaGrpcService : IService<IImpedimentoTarefaGrpcService>
{
    UnaryResult<OperationResult> AddAsync(CreateImpedimentoTarefaCommand command);

    UnaryResult<OperationResult> UpdateAsync(UpdateImpedimentoTarefaCommand command);

    UnaryResult<GetImpedimentoTarefaViewModel> GetAsync(GetImpedimentoTarefaQuery query);

    UnaryResult<ListImpedimentoTarefaViewModel> AllAsync(ListImpedimentoTarefaQuery query);

    UnaryResult<OperationResult> RemoveAsync(RemoveImpedimentoTarefaCommand command);

    UnaryResult<GetImpedimentoTarefaByTarefaViewModel> GetByTarefaAsync(GetImpedimentoTarefaByTarefaQuery query);
}