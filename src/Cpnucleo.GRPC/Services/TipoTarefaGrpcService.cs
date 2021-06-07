using Cpnucleo.Infra.CrossCutting.Util.Commands.TipoTarefa.CreateTipoTarefa;
using Cpnucleo.Infra.CrossCutting.Util.Commands.TipoTarefa.RemoveTipoTarefa;
using Cpnucleo.Infra.CrossCutting.Util.Commands.TipoTarefa.UpdateTipoTarefa;
using Cpnucleo.Infra.CrossCutting.Util.Interfaces;
using Cpnucleo.Infra.CrossCutting.Util.Queries.TipoTarefa.GetTipoTarefa;
using Cpnucleo.Infra.CrossCutting.Util.Queries.TipoTarefa.ListTipoTarefa;
using MagicOnion;
using MagicOnion.Server;
using MessagePipe;
using Microsoft.AspNetCore.Authorization;

namespace Cpnucleo.GRPC.Services
{
    [Authorize]
    public class TipoTarefaGrpcService : ServiceBase<ITipoTarefaGrpcService>, ITipoTarefaGrpcService
    {
        private readonly IAsyncRequestHandler<CreateTipoTarefaCommand, CreateTipoTarefaResponse> _createTipoTarefaCommand;
        private readonly IAsyncRequestHandler<ListTipoTarefaQuery, ListTipoTarefaResponse> _listTipoTarefaQuery;
        private readonly IAsyncRequestHandler<GetTipoTarefaQuery, GetTipoTarefaResponse> _getTipoTarefaQuery;
        private readonly IAsyncRequestHandler<RemoveTipoTarefaCommand, RemoveTipoTarefaResponse> _removeTipoTarefaCommand;
        private readonly IAsyncRequestHandler<UpdateTipoTarefaCommand, UpdateTipoTarefaResponse> _updateTipoTarefaCommand;

        public TipoTarefaGrpcService(IAsyncRequestHandler<CreateTipoTarefaCommand, CreateTipoTarefaResponse> createTipoTarefaCommand,
                                     IAsyncRequestHandler<ListTipoTarefaQuery, ListTipoTarefaResponse> listTipoTarefaQuery,
                                     IAsyncRequestHandler<GetTipoTarefaQuery, GetTipoTarefaResponse> getTipoTarefaQuery,
                                     IAsyncRequestHandler<RemoveTipoTarefaCommand, RemoveTipoTarefaResponse> removeTipoTarefaCommand,
                                     IAsyncRequestHandler<UpdateTipoTarefaCommand, UpdateTipoTarefaResponse> updateTipoTarefaCommand)
        {
            _createTipoTarefaCommand = createTipoTarefaCommand;
            _listTipoTarefaQuery = listTipoTarefaQuery;
            _getTipoTarefaQuery = getTipoTarefaQuery;
            _removeTipoTarefaCommand = removeTipoTarefaCommand;
            _updateTipoTarefaCommand = updateTipoTarefaCommand;
        }

        public async UnaryResult<CreateTipoTarefaResponse> AddAsync(CreateTipoTarefaCommand command)
        {
            return await _createTipoTarefaCommand.InvokeAsync(command);
        }

        public async UnaryResult<ListTipoTarefaResponse> AllAsync(ListTipoTarefaQuery query)
        {
            return await _listTipoTarefaQuery.InvokeAsync(query);
        }

        public async UnaryResult<GetTipoTarefaResponse> GetAsync(GetTipoTarefaQuery query)
        {
            return await _getTipoTarefaQuery.InvokeAsync(query);
        }

        public async UnaryResult<RemoveTipoTarefaResponse> RemoveAsync(RemoveTipoTarefaCommand command)
        {
            return await _removeTipoTarefaCommand.InvokeAsync(command);
        }

        public async UnaryResult<UpdateTipoTarefaResponse> UpdateAsync(UpdateTipoTarefaCommand command)
        {
            return await _updateTipoTarefaCommand.InvokeAsync(command);
        }
    }
}
