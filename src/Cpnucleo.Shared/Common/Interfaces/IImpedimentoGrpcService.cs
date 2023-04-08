using Cpnucleo.Shared.Commands.CreateImpedimento;
using Cpnucleo.Shared.Commands.RemoveImpedimento;
using Cpnucleo.Shared.Commands.UpdateImpedimento;
using Cpnucleo.Shared.Queries.GetImpedimento;
using Cpnucleo.Shared.Queries.ListImpedimento;

namespace Cpnucleo.Shared.Common.Interfaces;

public interface IImpedimentoGrpcService : IService<IImpedimentoGrpcService>
{
    UnaryResult<OperationResult> CreateImpedimento(CreateImpedimentoCommand command);

    UnaryResult<OperationResult> UpdateImpedimento(UpdateImpedimentoCommand command);

    UnaryResult<GetImpedimentoViewModel> GetImpedimento(GetImpedimentoQuery query);

    UnaryResult<ListImpedimentoViewModel> ListImpedimento(ListImpedimentoQuery query);

    UnaryResult<OperationResult> RemoveImpedimento(RemoveImpedimentoCommand command);
}