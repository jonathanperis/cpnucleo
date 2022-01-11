using Cpnucleo.Infra.CrossCutting.Util.Commands.RecursoProjeto;
using Cpnucleo.Infra.CrossCutting.Util.Queries.RecursoProjeto;
using Cpnucleo.Infra.CrossCutting.Util.ViewModels;

namespace Cpnucleo.GRPC.RecursoProjeto.Services;

[Authorize]
public class RecursoProjetoGrpcService : ServiceBase<IRecursoProjetoGrpcService>, IRecursoProjetoGrpcService
{
    private readonly IMediator _mediator;

    public RecursoProjetoGrpcService(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async UnaryResult<OperationResult> AddAsync(CreateRecursoProjetoCommand command)
    {
        return await _mediator.Send(command);
    }

    public async UnaryResult<IEnumerable<RecursoProjetoViewModel>> AllAsync(ListRecursoProjetoQuery query)
    {
        return await _mediator.Send(query);
    }

    public async UnaryResult<RecursoProjetoViewModel> GetAsync(GetRecursoProjetoQuery query)
    {
        return await _mediator.Send(query);
    }

    public async UnaryResult<IEnumerable<RecursoProjetoViewModel>> GetByProjetoAsync(GetByProjetoQuery query)
    {
        return await _mediator.Send(query);
    }

    public async UnaryResult<OperationResult> RemoveAsync(RemoveRecursoProjetoCommand command)
    {
        return await _mediator.Send(command);
    }

    public async UnaryResult<OperationResult> UpdateAsync(UpdateRecursoProjetoCommand command)
    {
        return await _mediator.Send(command);
    }
}
