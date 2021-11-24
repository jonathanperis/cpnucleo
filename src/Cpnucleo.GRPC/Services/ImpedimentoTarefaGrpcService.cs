using Cpnucleo.Infra.CrossCutting.Util.Commands.ImpedimentoTarefa;
using Cpnucleo.Infra.CrossCutting.Util.Queries.ImpedimentoTarefa;
using Cpnucleo.Infra.CrossCutting.Util.ViewModels;

namespace Cpnucleo.GRPC.Services;

[Authorize]
public class ImpedimentoTarefaGrpcService : ServiceBase<IImpedimentoTarefaGrpcService>, IImpedimentoTarefaGrpcService
{
    private readonly IMediator _mediator;

    public ImpedimentoTarefaGrpcService(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async UnaryResult<OperationResult> AddAsync(CreateImpedimentoTarefaCommand command)
    {
        return await _mediator.Send(command);
    }

    public async UnaryResult<IEnumerable<ImpedimentoTarefaViewModel>> AllAsync(ListImpedimentoTarefaQuery query)
    {
        return await _mediator.Send(query);
    }

    public async UnaryResult<ImpedimentoTarefaViewModel> GetAsync(GetImpedimentoTarefaQuery query)
    {
        return await _mediator.Send(query);
    }

    public async UnaryResult<IEnumerable<ImpedimentoTarefaViewModel>> GetByTarefaAsync(GetByTarefaQuery query)
    {
        return await _mediator.Send(query);
    }

    public async UnaryResult<OperationResult> RemoveAsync(RemoveImpedimentoTarefaCommand command)
    {
        return await _mediator.Send(command);
    }

    public async UnaryResult<OperationResult> UpdateAsync(UpdateImpedimentoTarefaCommand command)
    {
        return await _mediator.Send(command);
    }
}
