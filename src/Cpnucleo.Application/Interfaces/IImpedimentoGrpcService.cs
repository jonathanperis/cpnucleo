using Cpnucleo.Application.Commands.Impedimento.CreateImpedimento;
using Cpnucleo.Application.Commands.Impedimento.RemoveImpedimento;
using Cpnucleo.Application.Commands.Impedimento.UpdateImpedimento;
using Cpnucleo.Application.Queries.Impedimento.GetImpedimento;
using Cpnucleo.Application.Queries.Impedimento.ListImpedimento;

namespace Cpnucleo.Application.Interfaces;

public interface IImpedimentoGrpcService : IService<IImpedimentoGrpcService>
{
    UnaryResult<OperationResult> AddAsync(CreateImpedimentoCommand command);

    UnaryResult<OperationResult> UpdateAsync(UpdateImpedimentoCommand command);

    UnaryResult<GetImpedimentoViewModel> GetAsync(GetImpedimentoQuery query);

    UnaryResult<ListImpedimentoViewModel> AllAsync(ListImpedimentoQuery query);

    UnaryResult<OperationResult> RemoveAsync(RemoveImpedimentoCommand command);
}