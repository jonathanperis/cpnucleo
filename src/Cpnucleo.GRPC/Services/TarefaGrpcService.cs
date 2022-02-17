namespace Cpnucleo.GRPC.Services;

[Authorize]
public class TarefaGrpcService : ServiceBase<ITarefaGrpcService>, ITarefaGrpcService
{
    private readonly IMediator _mediator;

    public TarefaGrpcService(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async UnaryResult<OperationResult> CreateTarefa(CreateTarefaCommand command)
    {
        return await _mediator.Send(command);
    }

    public async UnaryResult<ListTarefaViewModel> ListTarefa(ListTarefaQuery query)
    {
        return await _mediator.Send(query);
    }

    public async UnaryResult<GetTarefaViewModel> GetTarefa(GetTarefaQuery query)
    {
        return await _mediator.Send(query);
    }

    public async UnaryResult<GetTarefaByRecursoViewModel> GetTarefaByRecurso(GetTarefaByRecursoQuery query)
    {
        return await _mediator.Send(query);
    }

    public async UnaryResult<OperationResult> RemoveTarefa(RemoveTarefaCommand command)
    {
        return await _mediator.Send(command);
    }

    public async UnaryResult<OperationResult> UpdateTarefa(UpdateTarefaCommand command)
    {
        return await _mediator.Send(command);
    }
}
