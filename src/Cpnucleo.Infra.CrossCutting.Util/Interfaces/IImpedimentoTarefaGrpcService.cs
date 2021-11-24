using Cpnucleo.Infra.CrossCutting.Util.Commands.ImpedimentoTarefa;
using Cpnucleo.Infra.CrossCutting.Util.Queries.ImpedimentoTarefa;

namespace Cpnucleo.Infra.CrossCutting.Util.Interfaces;

public interface IImpedimentoTarefaGrpcService : IService<IImpedimentoTarefaGrpcService>
{
    UnaryResult<OperationResult> AddAsync(CreateImpedimentoTarefaCommand command);

    UnaryResult<OperationResult> UpdateAsync(UpdateImpedimentoTarefaCommand command);

    UnaryResult<ImpedimentoTarefaViewModel> GetAsync(GetImpedimentoTarefaQuery query);

    UnaryResult<IEnumerable<ImpedimentoTarefaViewModel>> AllAsync(ListImpedimentoTarefaQuery query);

    UnaryResult<OperationResult> RemoveAsync(RemoveImpedimentoTarefaCommand command);

    UnaryResult<IEnumerable<ImpedimentoTarefaViewModel>> GetByTarefaAsync(GetByTarefaQuery query);
}