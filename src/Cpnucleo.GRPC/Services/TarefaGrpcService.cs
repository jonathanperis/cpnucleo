using Cpnucleo.Application.Commands.Tarefa.CreateTarefa;
using Cpnucleo.Application.Commands.Tarefa.RemoveTarefa;
using Cpnucleo.Application.Commands.Tarefa.UpdateTarefa;
using Cpnucleo.Application.Queries.Tarefa.GetTarefa;
using Cpnucleo.Application.Queries.Tarefa.GetTarefaByRecurso;
using Cpnucleo.Application.Queries.Tarefa.ListTarefa;

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

    public async UnaryResult<ListTarefaViewModel> AllAsync(ListTarefaQuery query)
    {
        return await _mediator.Send(query);
    }

    public async UnaryResult<GetTarefaViewModel> GetAsync(GetTarefaQuery query)
    {
        return await _mediator.Send(query);
    }

    public async UnaryResult<GetTarefaByRecursoViewModel> GetByRecursoAsync(GetTarefaByRecursoQuery query)
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
