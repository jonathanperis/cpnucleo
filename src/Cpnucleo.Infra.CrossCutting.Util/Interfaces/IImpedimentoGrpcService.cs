using Cpnucleo.Infra.CrossCutting.Util.Commands.Impedimento;
using Cpnucleo.Infra.CrossCutting.Util.Queries.Impedimento;

namespace Cpnucleo.Infra.CrossCutting.Util.Interfaces;

public interface IImpedimentoGrpcService : IService<IImpedimentoGrpcService>
{
    UnaryResult<OperationResult> AddAsync(CreateImpedimentoCommand command);

    UnaryResult<OperationResult> UpdateAsync(UpdateImpedimentoCommand command);

    UnaryResult<ImpedimentoViewModel> GetAsync(GetImpedimentoQuery query);

    UnaryResult<IEnumerable<ImpedimentoViewModel>> AllAsync(ListImpedimentoQuery query);

    UnaryResult<OperationResult> RemoveAsync(RemoveImpedimentoCommand command);
}