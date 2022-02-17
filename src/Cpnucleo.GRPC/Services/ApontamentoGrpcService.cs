namespace Cpnucleo.GRPC.Services;

[Authorize]
public class ApontamentoGrpcService : ServiceBase<IApontamentoGrpcService>, IApontamentoGrpcService
{
    private readonly IMediator _mediator;

    public ApontamentoGrpcService(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async UnaryResult<OperationResult> CreateApontamento(CreateApontamentoCommand command)
    {
        return await _mediator.Send(command);
    }

    public async UnaryResult<ListApontamentoViewModel> ListApontamento(ListApontamentoQuery query)
    {
        return await _mediator.Send(query);
    }

    public async UnaryResult<GetApontamentoViewModel> GetApontamento(GetApontamentoQuery query)
    {
        return await _mediator.Send(query);
    }

    public async UnaryResult<GetApontamentoByRecursoViewModel> GetApontamentoByRecurso(GetApontamentoByRecursoQuery query)
    {
        return await _mediator.Send(query);
    }

    public async UnaryResult<OperationResult> RemoveApontamento(RemoveApontamentoCommand command)
    {
        return await _mediator.Send(command);
    }

    public async UnaryResult<OperationResult> UpdateApontamento(UpdateApontamentoCommand command)
    {
        return await _mediator.Send(command);
    }
}
