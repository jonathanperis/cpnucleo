using Cpnucleo.Application.Commands.Apontamento.CreateApontamento;
using Cpnucleo.Application.Commands.Apontamento.RemoveApontamento;
using Cpnucleo.Application.Commands.Apontamento.UpdateApontamento;
using Cpnucleo.Application.Queries.Apontamento.GetApontamento;
using Cpnucleo.Application.Queries.Apontamento.GetApontamentoByRecurso;
using Cpnucleo.Application.Queries.Apontamento.ListApontamento;

namespace Cpnucleo.GRPC.Services;

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

    public async UnaryResult<ListApontamentoViewModel> AllAsync(ListApontamentoQuery query)
    {
        return await _mediator.Send(query);
    }

    public async UnaryResult<GetApontamentoViewModel> GetAsync(GetApontamentoQuery query)
    {
        return await _mediator.Send(query);
    }

    public async UnaryResult<GetApontamentoByRecursoViewModel> GetByRecursoAsync(GetApontamentoByRecursoQuery query)
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
