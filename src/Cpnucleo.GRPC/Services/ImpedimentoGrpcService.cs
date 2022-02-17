namespace Cpnucleo.GRPC.Services;

[Authorize]
public class ImpedimentoGrpcService : ServiceBase<IImpedimentoGrpcService>, IImpedimentoGrpcService
{
    private readonly IMediator _mediator;

    public ImpedimentoGrpcService(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async UnaryResult<OperationResult> CreateImpedimento(CreateImpedimentoCommand command)
    {
        return await _mediator.Send(command);
    }

    public async UnaryResult<ListImpedimentoViewModel> ListImpedimento(ListImpedimentoQuery query)
    {
        return await _mediator.Send(query);
    }

    public async UnaryResult<GetImpedimentoViewModel> GetImpedimento(GetImpedimentoQuery query)
    {
        return await _mediator.Send(query);
    }

    public async UnaryResult<OperationResult> RemoveImpedimento(RemoveImpedimentoCommand command)
    {
        return await _mediator.Send(command);
    }

    public async UnaryResult<OperationResult> UpdateImpedimento(UpdateImpedimentoCommand command)
    {
        return await _mediator.Send(command);
    }
}
