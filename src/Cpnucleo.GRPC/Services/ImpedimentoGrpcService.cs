using Cpnucleo.Application.Commands.Impedimento.CreateImpedimento;
using Cpnucleo.Application.Commands.Impedimento.RemoveImpedimento;
using Cpnucleo.Application.Commands.Impedimento.UpdateImpedimento;
using Cpnucleo.Application.Queries.Impedimento.GetImpedimento;
using Cpnucleo.Application.Queries.Impedimento.ListImpedimento;

namespace Cpnucleo.GRPC.Services;

[Authorize]
public class ImpedimentoGrpcService : ServiceBase<IImpedimentoGrpcService>, IImpedimentoGrpcService
{
    private readonly IMediator _mediator;

    public ImpedimentoGrpcService(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async UnaryResult<OperationResult> AddAsync(CreateImpedimentoCommand command)
    {
        return await _mediator.Send(command);
    }

    public async UnaryResult<ListImpedimentoViewModel> AllAsync(ListImpedimentoQuery query)
    {
        return await _mediator.Send(query);
    }

    public async UnaryResult<GetImpedimentoViewModel> GetAsync(GetImpedimentoQuery query)
    {
        return await _mediator.Send(query);
    }

    public async UnaryResult<OperationResult> RemoveAsync(RemoveImpedimentoCommand command)
    {
        return await _mediator.Send(command);
    }

    public async UnaryResult<OperationResult> UpdateAsync(UpdateImpedimentoCommand command)
    {
        return await _mediator.Send(command);
    }
}
