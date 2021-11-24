using Cpnucleo.Infra.CrossCutting.Util.Commands.TipoTarefa;
using Cpnucleo.Infra.CrossCutting.Util.Queries.TipoTarefa;
using Cpnucleo.Infra.CrossCutting.Util.ViewModels;

namespace Cpnucleo.GRPC.Services;

[Authorize]
public class TipoTarefaGrpcService : ServiceBase<ITipoTarefaGrpcService>, ITipoTarefaGrpcService
{
    private readonly IMediator _mediator;

    public TipoTarefaGrpcService(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async UnaryResult<OperationResult> AddAsync(CreateTipoTarefaCommand command)
    {
        return await _mediator.Send(command);
    }

    public async UnaryResult<IEnumerable<TipoTarefaViewModel>> AllAsync(ListTipoTarefaQuery query)
    {
        return await _mediator.Send(query);
    }

    public async UnaryResult<TipoTarefaViewModel> GetAsync(GetTipoTarefaQuery query)
    {
        return await _mediator.Send(query);
    }

    public async UnaryResult<OperationResult> RemoveAsync(RemoveTipoTarefaCommand command)
    {
        return await _mediator.Send(command);
    }

    public async UnaryResult<OperationResult> UpdateAsync(UpdateTipoTarefaCommand command)
    {
        return await _mediator.Send(command);
    }
}
