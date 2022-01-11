using Cpnucleo.Infra.CrossCutting.Util.Commands.RecursoTarefa;
using Cpnucleo.Infra.CrossCutting.Util.Queries.RecursoTarefa;
using Cpnucleo.Infra.CrossCutting.Util.ViewModels;

namespace Cpnucleo.GRPC.RecursoTarefa.Services;

[Authorize]
public class RecursoTarefaGrpcService : ServiceBase<IRecursoTarefaGrpcService>, IRecursoTarefaGrpcService
{
    private readonly IMediator _mediator;

    public RecursoTarefaGrpcService(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async UnaryResult<OperationResult> AddAsync(CreateRecursoTarefaCommand command)
    {
        return await _mediator.Send(command);
    }

    public async UnaryResult<IEnumerable<RecursoTarefaViewModel>> AllAsync(ListRecursoTarefaQuery query)
    {
        return await _mediator.Send(query);
    }

    public async UnaryResult<RecursoTarefaViewModel> GetAsync(GetRecursoTarefaQuery query)
    {
        return await _mediator.Send(query);
    }

    public async UnaryResult<IEnumerable<RecursoTarefaViewModel>> GetByTarefaAsync(GetByTarefaQuery query)
    {
        return await _mediator.Send(query);
    }

    public async UnaryResult<OperationResult> RemoveAsync(RemoveRecursoTarefaCommand command)
    {
        return await _mediator.Send(command);
    }

    public async UnaryResult<OperationResult> UpdateAsync(UpdateRecursoTarefaCommand command)
    {
        return await _mediator.Send(command);
    }
}
