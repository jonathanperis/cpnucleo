using Cpnucleo.Application.Commands.RecursoTarefa.CreateRecursoTarefa;
using Cpnucleo.Application.Commands.RecursoTarefa.RemoveRecursoTarefa;
using Cpnucleo.Application.Commands.RecursoTarefa.UpdateRecursoTarefa;
using Cpnucleo.Application.Queries.RecursoTarefa.GetByTarefa;
using Cpnucleo.Application.Queries.RecursoTarefa.GetRecursoTarefa;
using Cpnucleo.Application.Queries.RecursoTarefa.ListRecursoTarefa;

namespace Cpnucleo.GRPC.Services;

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

    public async UnaryResult<ListRecursoTarefaViewModel> AllAsync(ListRecursoTarefaQuery query)
    {
        return await _mediator.Send(query);
    }

    public async UnaryResult<GetRecursoTarefaViewModel> GetAsync(GetRecursoTarefaQuery query)
    {
        return await _mediator.Send(query);
    }

    public async UnaryResult<GetRecursoTarefaByTarefaViewModel> GetByTarefaAsync(GetRecursoTarefaByTarefaQuery query)
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
