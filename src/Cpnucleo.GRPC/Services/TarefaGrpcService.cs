using Cpnucleo.Infra.CrossCutting.Util.Commands.Tarefa;
using Cpnucleo.Infra.CrossCutting.Util.Queries.Tarefa;
using Cpnucleo.Infra.CrossCutting.Util.ViewModels;

namespace Cpnucleo.GRPC.Services;

[Authorize]
public class TarefaGrpcService : ServiceBase<ITarefaGrpcService>, ITarefaGrpcService
{
    private readonly IMediator _mediator;

    public TarefaGrpcService(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async UnaryResult<OperationResult> AddAsync(CreateTarefaCommand command)
    {
        return await _mediator.Send(command);
    }

    public async UnaryResult<IEnumerable<TarefaViewModel>> AllAsync(ListTarefaQuery query)
    {
        return await _mediator.Send(query);
    }

    public async UnaryResult<TarefaViewModel> GetAsync(GetTarefaQuery query)
    {
        return await _mediator.Send(query);
    }

    public async UnaryResult<IEnumerable<TarefaViewModel>> GetByRecursoAsync(GetByRecursoQuery query)
    {
        return await _mediator.Send(query);
    }

    public async UnaryResult<OperationResult> RemoveAsync(RemoveTarefaCommand command)
    {
        return await _mediator.Send(command);
    }

    public async UnaryResult<OperationResult> UpdateAsync(UpdateTarefaCommand command)
    {
        return await _mediator.Send(command);
    }
}
