using Cpnucleo.Infra.CrossCutting.Util.Commands.RecursoTarefa.CreateRecursoTarefa;
using Cpnucleo.Infra.CrossCutting.Util.Commands.RecursoTarefa.RemoveRecursoTarefa;
using Cpnucleo.Infra.CrossCutting.Util.Commands.RecursoTarefa.UpdateRecursoTarefa;
using Cpnucleo.Infra.CrossCutting.Util.Interfaces;
using Cpnucleo.Infra.CrossCutting.Util.Queries.RecursoTarefa.GetByTarefa;
using Cpnucleo.Infra.CrossCutting.Util.Queries.RecursoTarefa.GetRecursoTarefa;
using Cpnucleo.Infra.CrossCutting.Util.Queries.RecursoTarefa.ListRecursoTarefa;
using MagicOnion;
using MagicOnion.Server;
using MessagePipe;
using Microsoft.AspNetCore.Authorization;

namespace Cpnucleo.GRPC.Services
{
    [Authorize]
    public class RecursoTarefaGrpcService : ServiceBase<IRecursoTarefaGrpcService>, IRecursoTarefaGrpcService
    {
        private readonly IAsyncRequestHandler<CreateRecursoTarefaCommand, CreateRecursoTarefaResponse> _createRecursoTarefaCommand;
        private readonly IAsyncRequestHandler<ListRecursoTarefaQuery, ListRecursoTarefaResponse> _listRecursoTarefaQuery;
        private readonly IAsyncRequestHandler<GetRecursoTarefaQuery, GetRecursoTarefaResponse> _getRecursoTarefaQuery;
        private readonly IAsyncRequestHandler<GetByTarefaQuery, GetByTarefaResponse> _getByTarefaQuery;
        private readonly IAsyncRequestHandler<RemoveRecursoTarefaCommand, RemoveRecursoTarefaResponse> _removeRecursoTarefaCommand;
        private readonly IAsyncRequestHandler<UpdateRecursoTarefaCommand, UpdateRecursoTarefaResponse> _updateRecursoTarefaCommand;

        public RecursoTarefaGrpcService(IAsyncRequestHandler<CreateRecursoTarefaCommand, CreateRecursoTarefaResponse> createRecursoTarefaCommand,
                                        IAsyncRequestHandler<ListRecursoTarefaQuery, ListRecursoTarefaResponse> listRecursoTarefaQuery,
                                        IAsyncRequestHandler<GetRecursoTarefaQuery, GetRecursoTarefaResponse> getRecursoTarefaQuery,
                                        IAsyncRequestHandler<GetByTarefaQuery, GetByTarefaResponse> getByTarefaQuery,
                                        IAsyncRequestHandler<RemoveRecursoTarefaCommand, RemoveRecursoTarefaResponse> removeRecursoTarefaCommand,
                                        IAsyncRequestHandler<UpdateRecursoTarefaCommand, UpdateRecursoTarefaResponse> updateRecursoTarefaCommand)
        {
            _createRecursoTarefaCommand = createRecursoTarefaCommand;
            _listRecursoTarefaQuery = listRecursoTarefaQuery;
            _getRecursoTarefaQuery = getRecursoTarefaQuery;
            _getByTarefaQuery = getByTarefaQuery;
            _removeRecursoTarefaCommand = removeRecursoTarefaCommand;
            _updateRecursoTarefaCommand = updateRecursoTarefaCommand;
        }

        public async UnaryResult<CreateRecursoTarefaResponse> AddAsync(CreateRecursoTarefaCommand command)
        {
            return await _createRecursoTarefaCommand.InvokeAsync(command);
        }

        public async UnaryResult<ListRecursoTarefaResponse> AllAsync(ListRecursoTarefaQuery query)
        {
            return await _listRecursoTarefaQuery.InvokeAsync(query);
        }

        public async UnaryResult<GetRecursoTarefaResponse> GetAsync(GetRecursoTarefaQuery query)
        {
            return await _getRecursoTarefaQuery.InvokeAsync(query);
        }

        public async UnaryResult<GetByTarefaResponse> GetByTarefaAsync(GetByTarefaQuery query)
        {
            return await _getByTarefaQuery.InvokeAsync(query);
        }

        public async UnaryResult<RemoveRecursoTarefaResponse> RemoveAsync(RemoveRecursoTarefaCommand command)
        {
            return await _removeRecursoTarefaCommand.InvokeAsync(command);
        }

        public async UnaryResult<UpdateRecursoTarefaResponse> UpdateAsync(UpdateRecursoTarefaCommand command)
        {
            return await _updateRecursoTarefaCommand.InvokeAsync(command);
        }
    }
}
