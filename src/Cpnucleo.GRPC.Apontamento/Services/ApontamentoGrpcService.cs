using Cpnucleo.Infra.CrossCutting.Util.Commands.Apontamento;
using Cpnucleo.Infra.CrossCutting.Util.Queries.Apontamento;
using Cpnucleo.Infra.CrossCutting.Util.ViewModels;

namespace Cpnucleo.GRPC.Apontamento.Services;

[Authorize]
public class ApontamentoGrpcService : ServiceBase<IApontamentoGrpcService>, IApontamentoGrpcService
{
    private readonly IMediator _mediator;

    public ApontamentoGrpcService(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async UnaryResult<OperationResult> AddAsync(CreateApontamentoCommand command)
    {
        return await _mediator.Send(command);
    }

    public async UnaryResult<IEnumerable<ApontamentoViewModel>> AllAsync(ListApontamentoQuery query)
    {
        return await _mediator.Send(query);
    }

    public async UnaryResult<ApontamentoViewModel> GetAsync(GetApontamentoQuery query)
    {
        return await _mediator.Send(query);
    }

    public async UnaryResult<IEnumerable<ApontamentoViewModel>> GetByRecursoAsync(GetByRecursoQuery query)
    {
        return await _mediator.Send(query);
    }

    public async UnaryResult<OperationResult> RemoveAsync(RemoveApontamentoCommand command)
    {
        return await _mediator.Send(command);
    }

    public async UnaryResult<OperationResult> UpdateAsync(UpdateApontamentoCommand command)
    {
        return await _mediator.Send(command);
    }
}
