using Cpnucleo.Application.Commands.Recurso.CreateRecurso;
using Cpnucleo.Application.Commands.Recurso.RemoveRecurso;
using Cpnucleo.Application.Commands.Recurso.UpdateRecurso;
using Cpnucleo.Application.Queries.Recurso.GetRecurso;
using Cpnucleo.Application.Queries.Recurso.ListRecurso;

namespace Cpnucleo.GRPC.Services;

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

    public async UnaryResult<ListRecursoViewModel> AllAsync(ListRecursoQuery query)
    {
        return await _mediator.Send(query);
    }

    public async UnaryResult<GetRecursoViewModel> GetAsync(GetRecursoQuery query)
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
