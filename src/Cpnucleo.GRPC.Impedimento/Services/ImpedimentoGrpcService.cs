using Cpnucleo.Infra.CrossCutting.Util.Commands.Impedimento;
using Cpnucleo.Infra.CrossCutting.Util.Queries.Impedimento;
using Cpnucleo.Infra.CrossCutting.Util.ViewModels;

namespace Cpnucleo.GRPC.Impedimento.Services;

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

    public async UnaryResult<IEnumerable<ImpedimentoViewModel>> AllAsync(ListImpedimentoQuery query)
    {
        return await _mediator.Send(query);
    }

    public async UnaryResult<ImpedimentoViewModel> GetAsync(GetImpedimentoQuery query)
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
