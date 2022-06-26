namespace Cpnucleo.Shared.Common.Interfaces;

public interface IImpedimentoGrpcService : IService<IImpedimentoGrpcService>
{
    UnaryResult<OperationResult> CreateImpedimento(CreateImpedimentoCommand command);

    UnaryResult<OperationResult> UpdateImpedimento(UpdateImpedimentoCommand command);

    UnaryResult<GetImpedimentoViewModel> GetImpedimento(GetImpedimentoQuery query);

    UnaryResult<ListImpedimentoViewModel> ListImpedimento(ListImpedimentoQuery query);

    UnaryResult<OperationResult> RemoveImpedimento(RemoveImpedimentoCommand command);
}