using Cpnucleo.Application.Commands.Projeto.CreateProjeto;
using Cpnucleo.Application.Commands.Projeto.RemoveProjeto;
using Cpnucleo.Application.Commands.Projeto.UpdateProjeto;
using Cpnucleo.Application.Queries.Projeto.GetProjeto;
using Cpnucleo.Application.Queries.Projeto.ListProjeto;

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

    public async UnaryResult<ListProjetoViewModel> AllAsync(ListProjetoQuery query)
    {
        return await _mediator.Send(query);
    }

    public async UnaryResult<GetProjetoViewModel> GetAsync(GetProjetoQuery query)
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
