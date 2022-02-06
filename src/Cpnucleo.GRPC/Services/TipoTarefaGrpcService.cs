using Cpnucleo.Application.Commands.TipoTarefa.CreateTipoTarefa;
using Cpnucleo.Application.Commands.TipoTarefa.RemoveTipoTarefa;
using Cpnucleo.Application.Commands.TipoTarefa.UpdateTipoTarefa;
using Cpnucleo.Application.Queries.TipoTarefa.GetTipoTarefa;
using Cpnucleo.Application.Queries.TipoTarefa.ListTipoTarefa;

namespace Cpnucleo.GRPC.Services;

[Authorize]
public class TipoTarefaGrpcService : ServiceBase<ITipoTarefaGrpcService>, ITipoTarefaGrpcService
{
    private readonly IMediator _mediator;

    public TipoTarefaGrpcService(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async UnaryResult<OperationResult> CreateTipoTarefa(CreateTipoTarefaCommand command)
    {
        return await _mediator.Send(command);
    }

    public async UnaryResult<ListTipoTarefaViewModel> ListTipoTarefa(ListTipoTarefaQuery query)
    {
        return await _mediator.Send(query);
    }

    public async UnaryResult<GetTipoTarefaViewModel> GetTipoTarefa(GetTipoTarefaQuery query)
    {
        return await _mediator.Send(query);
    }

    public async UnaryResult<OperationResult> RemoveTipoTarefa(RemoveTipoTarefaCommand command)
    {
        return await _mediator.Send(command);
    }

    public async UnaryResult<OperationResult> UpdateTipoTarefa(UpdateTipoTarefaCommand command)
    {
        return await _mediator.Send(command);
    }
}
