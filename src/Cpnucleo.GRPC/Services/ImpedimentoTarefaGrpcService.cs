using Cpnucleo.Application.Commands.ImpedimentoTarefa.CreateImpedimentoTarefa;
using Cpnucleo.Application.Commands.ImpedimentoTarefa.RemoveImpedimentoTarefa;
using Cpnucleo.Application.Commands.ImpedimentoTarefa.UpdateImpedimentoTarefa;
using Cpnucleo.Application.Queries.ImpedimentoTarefa.GetImpedimentoTarefa;
using Cpnucleo.Application.Queries.ImpedimentoTarefa.GetImpedimentoTarefaByTarefa;
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

    public async UnaryResult<OperationResult> CreateImpedimentoTarefa(CreateImpedimentoTarefaCommand command)
    {
        return await _mediator.Send(command);
    }

    public async UnaryResult<ListImpedimentoTarefaViewModel> ListImpedimentoTarefa(ListImpedimentoTarefaQuery query)
    {
        return await _mediator.Send(query);
    }

    public async UnaryResult<GetImpedimentoTarefaViewModel> GetImpedimentoTarefa(GetImpedimentoTarefaQuery query)
    {
        return await _mediator.Send(query);
    }

    public async UnaryResult<GetImpedimentoTarefaByTarefaViewModel> GetImpedimentoTarefaByTarefa(GetImpedimentoTarefaByTarefaQuery query)
    {
        return await _mediator.Send(query);
    }

    public async UnaryResult<OperationResult> RemoveImpedimentoTarefa(RemoveImpedimentoTarefaCommand command)
    {
        return await _mediator.Send(command);
    }

    public async UnaryResult<OperationResult> UpdateImpedimentoTarefa(UpdateImpedimentoTarefaCommand command)
    {
        return await _mediator.Send(command);
    }
}
