using Cpnucleo.Infra.CrossCutting.Util.Commands.Projeto;
using Cpnucleo.Infra.CrossCutting.Util.Queries.Projeto;
using Cpnucleo.Infra.CrossCutting.Util.ViewModels;

namespace Cpnucleo.GRPC.Services;

[Authorize]
public class ProjetoGrpcService : ServiceBase<IProjetoGrpcService>, IProjetoGrpcService
{
    private readonly IAsyncRequestHandler<CreateProjetoCommand, OperationResult> _createProjetoCommand;
    private readonly IAsyncRequestHandler<ListProjetoQuery, IEnumerable<ProjetoViewModel>> _listProjetoQuery;
    private readonly IAsyncRequestHandler<GetProjetoQuery, ProjetoViewModel> _getProjetoQuery;
    private readonly IAsyncRequestHandler<RemoveProjetoCommand, OperationResult> _removeProjetoCommand;
    private readonly IAsyncRequestHandler<UpdateProjetoCommand, OperationResult> _updateProjetoCommand;

    public ProjetoGrpcService(IAsyncRequestHandler<CreateProjetoCommand, OperationResult> createProjetoCommand,
                              IAsyncRequestHandler<ListProjetoQuery, IEnumerable<ProjetoViewModel>> listProjetoQuery,
                              IAsyncRequestHandler<GetProjetoQuery, ProjetoViewModel> getProjetoQuery,
                              IAsyncRequestHandler<RemoveProjetoCommand, OperationResult> removeProjetoCommand,
                              IAsyncRequestHandler<UpdateProjetoCommand, OperationResult> updateProjetoCommand)
    {
        _createProjetoCommand = createProjetoCommand;
        _listProjetoQuery = listProjetoQuery;
        _getProjetoQuery = getProjetoQuery;
        _removeProjetoCommand = removeProjetoCommand;
        _updateProjetoCommand = updateProjetoCommand;
    }

    public async UnaryResult<OperationResult> AddAsync(CreateProjetoCommand command)
    {
        return await _createProjetoCommand.InvokeAsync(command);
    }

    public async UnaryResult<IEnumerable<ProjetoViewModel>> AllAsync(ListProjetoQuery query)
    {
        return await _listProjetoQuery.InvokeAsync(query);
    }

    public async UnaryResult<ProjetoViewModel> GetAsync(GetProjetoQuery query)
    {
        return await _getProjetoQuery.InvokeAsync(query);
    }

    public async UnaryResult<OperationResult> RemoveAsync(RemoveProjetoCommand command)
    {
        return await _removeProjetoCommand.InvokeAsync(command);
    }

    public async UnaryResult<OperationResult> UpdateAsync(UpdateProjetoCommand command)
    {
        return await _updateProjetoCommand.InvokeAsync(command);
    }
}
