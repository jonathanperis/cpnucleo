namespace Cpnucleo.GRPC.Services;

[Authorize]
public class RecursoProjetoGrpcService : ServiceBase<IRecursoProjetoGrpcService>, IRecursoProjetoGrpcService
{
    private readonly IMediator _mediator;

    public RecursoProjetoGrpcService(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async UnaryResult<OperationResult> CreateRecursoProjeto(CreateRecursoProjetoCommand command)
    {
        return await _mediator.Send(command);
    }

    public async UnaryResult<ListRecursoProjetoViewModel> ListRecursoProjeto(ListRecursoProjetoQuery query)
    {
        return await _mediator.Send(query);
    }

    public async UnaryResult<GetRecursoProjetoViewModel> GetRecursoProjeto(GetRecursoProjetoQuery query)
    {
        return await _mediator.Send(query);
    }

    public async UnaryResult<GetRecursoProjetoByProjetoViewModel> GetRecursoProjetoByProjeto(GetRecursoProjetoByProjetoQuery query)
    {
        return await _mediator.Send(query);
    }

    public async UnaryResult<OperationResult> RemoveRecursoProjeto(RemoveRecursoProjetoCommand command)
    {
        return await _mediator.Send(command);
    }

    public async UnaryResult<OperationResult> UpdateRecursoProjeto(UpdateRecursoProjetoCommand command)
    {
        return await _mediator.Send(command);
    }
}
