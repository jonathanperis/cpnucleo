namespace Cpnucleo.GRPC.Services;

[Authorize]
public class RecursoTarefaGrpcService : ServiceBase<IRecursoTarefaGrpcService>, IRecursoTarefaGrpcService
{
    private readonly IMediator _mediator;

    public RecursoTarefaGrpcService(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async UnaryResult<OperationResult> CreateRecursoTarefa(CreateRecursoTarefaCommand command)
    {
        return await _mediator.Send(command);
    }

    public async UnaryResult<ListRecursoTarefaViewModel> ListRecursoTarefa(ListRecursoTarefaQuery query)
    {
        return await _mediator.Send(query);
    }

    public async UnaryResult<GetRecursoTarefaViewModel> GetRecursoTarefa(GetRecursoTarefaQuery query)
    {
        return await _mediator.Send(query);
    }

    public async UnaryResult<GetRecursoTarefaByTarefaViewModel> GetRecursoTarefaByTarefa(GetRecursoTarefaByTarefaQuery query)
    {
        return await _mediator.Send(query);
    }

    public async UnaryResult<OperationResult> RemoveRecursoTarefa(RemoveRecursoTarefaCommand command)
    {
        return await _mediator.Send(command);
    }

    public async UnaryResult<OperationResult> UpdateRecursoTarefa(UpdateRecursoTarefaCommand command)
    {
        return await _mediator.Send(command);
    }
}
