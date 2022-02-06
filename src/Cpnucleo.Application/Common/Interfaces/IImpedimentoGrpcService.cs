using Cpnucleo.Application.Commands.Impedimento.CreateImpedimento;
using Cpnucleo.Application.Commands.Impedimento.RemoveImpedimento;
using Cpnucleo.Application.Commands.Impedimento.UpdateImpedimento;
using Cpnucleo.Application.Queries.Impedimento.GetImpedimento;
using Cpnucleo.Application.Queries.Impedimento.ListImpedimento;
using MagicOnion;

namespace Cpnucleo.Application.Interfaces;

public interface IImpedimentoGrpcService : IService<IImpedimentoGrpcService>
{
    UnaryResult<OperationResult> CreateImpedimento(CreateImpedimentoCommand command);

    UnaryResult<OperationResult> UpdateImpedimento(UpdateImpedimentoCommand command);

    UnaryResult<GetImpedimentoViewModel> GetImpedimento(GetImpedimentoQuery query);

    UnaryResult<ListImpedimentoViewModel> ListImpedimento(ListImpedimentoQuery query);

    UnaryResult<OperationResult> RemoveImpedimento(RemoveImpedimentoCommand command);
}