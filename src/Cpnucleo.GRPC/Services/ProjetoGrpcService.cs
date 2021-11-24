using Cpnucleo.Infra.CrossCutting.Util.Commands.Projeto;
using Cpnucleo.Infra.CrossCutting.Util.Queries.Projeto;
using Cpnucleo.Infra.CrossCutting.Util.ViewModels;

namespace Cpnucleo.GRPC.Services;

[Authorize]
public class ProjetoGrpcService : ServiceBase<IProjetoGrpcService>, IProjetoGrpcService
{
    private readonly IMediator _mediator;

    public ProjetoGrpcService(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async UnaryResult<OperationResult> AddAsync(CreateProjetoCommand command)
    {
        return await _mediator.Send(command);
    }

    public async UnaryResult<IEnumerable<ProjetoViewModel>> AllAsync(ListProjetoQuery query)
    {
        return await _mediator.Send(query);
    }

    public async UnaryResult<ProjetoViewModel> GetAsync(GetProjetoQuery query)
    {
        return await _mediator.Send(query);
    }

    public async UnaryResult<OperationResult> RemoveAsync(RemoveProjetoCommand command)
    {
        return await _mediator.Send(command);
    }

    public async UnaryResult<OperationResult> UpdateAsync(UpdateProjetoCommand command)
    {
        return await _mediator.Send(command);
    }
}
