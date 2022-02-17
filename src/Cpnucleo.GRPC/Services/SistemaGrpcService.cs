namespace Cpnucleo.GRPC.Services;

[Authorize]
public class SistemaGrpcService : ServiceBase<ISistemaGrpcService>, ISistemaGrpcService
{
    private readonly IMediator _mediator;

    public SistemaGrpcService(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async UnaryResult<OperationResult> CreateSistema(CreateSistemaCommand command)
    {
        return await _mediator.Send(command);
    }

    public async UnaryResult<ListSistemaViewModel> ListSistema(ListSistemaQuery query)
    {
        return await _mediator.Send(query);
    }

    public async UnaryResult<GetSistemaViewModel> GetSistema(GetSistemaQuery query)
    {
        return await _mediator.Send(query);
    }

    public async UnaryResult<OperationResult> RemoveSistema(RemoveSistemaCommand command)
    {
        return await _mediator.Send(command);
    }

    public async UnaryResult<OperationResult> UpdateSistema(UpdateSistemaCommand command)
    {
        return await _mediator.Send(command);
    }
}
