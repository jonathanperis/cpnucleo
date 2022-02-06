using Cpnucleo.Application.Commands.Sistema.CreateSistema;
using Cpnucleo.Application.Commands.Sistema.RemoveSistema;
using Cpnucleo.Application.Commands.Sistema.UpdateSistema;
using Cpnucleo.Application.Queries.Sistema.GetSistema;
using Cpnucleo.Application.Queries.Sistema.ListSistema;

namespace Cpnucleo.GRPC.Services;

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

    public async UnaryResult<ListSistemaViewModel> AllAsync(ListSistemaQuery query)
    {
        return await _mediator.Send(query);
    }

    public async UnaryResult<GetSistemaViewModel> GetAsync(GetSistemaQuery query)
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
