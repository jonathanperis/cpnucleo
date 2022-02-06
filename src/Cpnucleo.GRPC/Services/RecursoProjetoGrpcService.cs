using Cpnucleo.Application.Commands.RecursoProjeto.CreateRecursoProjeto;
using Cpnucleo.Application.Commands.RecursoProjeto.RemoveRecursoProjeto;
using Cpnucleo.Application.Commands.RecursoProjeto.UpdateRecursoProjeto;
using Cpnucleo.Application.Queries.RecursoProjeto.GetByProjeto;
using Cpnucleo.Application.Queries.RecursoProjeto.GetRecursoProjeto;
using Cpnucleo.Application.Queries.RecursoProjeto.ListRecursoProjeto;

namespace Cpnucleo.GRPC.Services;

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

    public async UnaryResult<ListRecursoProjetoViewModel> AllAsync(ListRecursoProjetoQuery query)
    {
        return await _mediator.Send(query);
    }

    public async UnaryResult<GetRecursoProjetoViewModel> GetAsync(GetRecursoProjetoQuery query)
    {
        return await _mediator.Send(query);
    }

    public async UnaryResult<GetRecursoProjetoByProjetoViewModel> GetByProjetoAsync(GetRecursoProjetoByProjetoQuery query)
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
