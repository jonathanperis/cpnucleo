using Cpnucleo.Application.Commands.ImpedimentoTarefa.CreateImpedimentoTarefa;
using Cpnucleo.Application.Commands.ImpedimentoTarefa.RemoveImpedimentoTarefa;
using Cpnucleo.Application.Commands.ImpedimentoTarefa.UpdateImpedimentoTarefa;
using Cpnucleo.Application.Queries.ImpedimentoTarefa.GetByTarefa;
using Cpnucleo.Application.Queries.ImpedimentoTarefa.GetImpedimentoTarefa;
using Cpnucleo.Application.Queries.ImpedimentoTarefa.ListImpedimentoTarefa;

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

    public async UnaryResult<ListImpedimentoTarefaViewModel> AllAsync(ListImpedimentoTarefaQuery query)
    {
        return await _mediator.Send(query);
    }

    public async UnaryResult<GetImpedimentoTarefaViewModel> GetAsync(GetImpedimentoTarefaQuery query)
    {
        return await _mediator.Send(query);
    }

    public async UnaryResult<GetImpedimentoTarefaByTarefaViewModel> GetByTarefaAsync(GetImpedimentoTarefaByTarefaQuery query)
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
