namespace Cpnucleo.GRPC.Services;

[Authorize]
public class RecursoGrpcService : ServiceBase<IRecursoGrpcService>, IRecursoGrpcService
{
    private readonly IMediator _mediator;

    public RecursoGrpcService(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async UnaryResult<OperationResult> CreateRecurso(CreateRecursoCommand command)
    {
        return await _mediator.Send(command);
    }

    public async UnaryResult<ListRecursoViewModel> ListRecurso(ListRecursoQuery query)
    {
        return await _mediator.Send(query);
    }

    public async UnaryResult<GetRecursoViewModel> GetRecurso(GetRecursoQuery query)
    {
        return await _mediator.Send(query);
    }

    public async UnaryResult<OperationResult> RemoveRecurso(RemoveRecursoCommand command)
    {
        return await _mediator.Send(command);
    }

    public async UnaryResult<OperationResult> UpdateRecurso(UpdateRecursoCommand command)
    {
        return await _mediator.Send(command);
    }
}
