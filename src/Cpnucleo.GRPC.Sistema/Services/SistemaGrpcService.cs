using Cpnucleo.Infra.CrossCutting.Util.Commands.Sistema;
using Cpnucleo.Infra.CrossCutting.Util.Queries.Sistema;
using Cpnucleo.Infra.CrossCutting.Util.ViewModels;

namespace Cpnucleo.GRPC.Sistema.Services;

[Authorize]
public class SistemaGrpcService : ServiceBase<ISistemaGrpcService>, ISistemaGrpcService
{
    private readonly IMediator _mediator;

    public SistemaGrpcService(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async UnaryResult<OperationResult> AddAsync(CreateSistemaCommand command)
    {
        return await _mediator.Send(command);
    }

    public async UnaryResult<IEnumerable<SistemaViewModel>> AllAsync(ListSistemaQuery query)
    {
        return await _mediator.Send(query);
    }

    public async UnaryResult<SistemaViewModel> GetAsync(GetSistemaQuery query)
    {
        return await _mediator.Send(query);
    }

    public async UnaryResult<OperationResult> RemoveAsync(RemoveSistemaCommand command)
    {
        return await _mediator.Send(command);
    }

    public async UnaryResult<OperationResult> UpdateAsync(UpdateSistemaCommand command)
    {
        return await _mediator.Send(command);
    }
}
