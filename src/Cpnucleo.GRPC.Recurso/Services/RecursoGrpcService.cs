using Cpnucleo.Infra.CrossCutting.Util.Commands.Recurso;
using Cpnucleo.Infra.CrossCutting.Util.Queries.Recurso;
using Cpnucleo.Infra.CrossCutting.Util.ViewModels;

namespace Cpnucleo.GRPC.Recurso.Services;

[Authorize]
public class RecursoGrpcService : ServiceBase<IRecursoGrpcService>, IRecursoGrpcService
{
    private readonly IMediator _mediator;

    public RecursoGrpcService(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async UnaryResult<OperationResult> AddAsync(CreateRecursoCommand command)
    {
        return await _mediator.Send(command);
    }

    public async UnaryResult<IEnumerable<RecursoViewModel>> AllAsync(ListRecursoQuery query)
    {
        return await _mediator.Send(query);
    }

    public async UnaryResult<RecursoViewModel> GetAsync(GetRecursoQuery query)
    {
        return await _mediator.Send(query);
    }

    public async UnaryResult<OperationResult> RemoveAsync(RemoveRecursoCommand command)
    {
        return await _mediator.Send(command);
    }

    public async UnaryResult<OperationResult> UpdateAsync(UpdateRecursoCommand command)
    {
        return await _mediator.Send(command);
    }
}
