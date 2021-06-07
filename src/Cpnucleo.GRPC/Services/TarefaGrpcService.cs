using Cpnucleo.Infra.CrossCutting.Util.Commands.Tarefa.CreateTarefa;
using Cpnucleo.Infra.CrossCutting.Util.Commands.Tarefa.RemoveTarefa;
using Cpnucleo.Infra.CrossCutting.Util.Commands.Tarefa.UpdateTarefa;
using Cpnucleo.Infra.CrossCutting.Util.Interfaces;
using Cpnucleo.Infra.CrossCutting.Util.Queries.Tarefa.GetByRecurso;
using Cpnucleo.Infra.CrossCutting.Util.Queries.Tarefa.GetTarefa;
using Cpnucleo.Infra.CrossCutting.Util.Queries.Tarefa.ListTarefa;
using MagicOnion;
using MagicOnion.Server;
using MessagePipe;
using Microsoft.AspNetCore.Authorization;

namespace Cpnucleo.GRPC.Services
{
    [Authorize]
    public class TarefaGrpcService : ServiceBase<ITarefaGrpcService>, ITarefaGrpcService
    {
        private readonly IAsyncRequestHandler<CreateTarefaCommand, CreateTarefaResponse> _createTarefaCommand;
        private readonly IAsyncRequestHandler<ListTarefaQuery, ListTarefaResponse> _listTarefaQuery;
        private readonly IAsyncRequestHandler<GetTarefaQuery, GetTarefaResponse> _getTarefaQuery;
        private readonly IAsyncRequestHandler<GetByRecursoQuery, GetByRecursoResponse> _getByRecursoQuery;
        private readonly IAsyncRequestHandler<RemoveTarefaCommand, RemoveTarefaResponse> _removeTarefaCommand;
        private readonly IAsyncRequestHandler<UpdateTarefaCommand, UpdateTarefaResponse> _updateTarefaCommand;

        public TarefaGrpcService(IAsyncRequestHandler<CreateTarefaCommand, CreateTarefaResponse> createTarefaCommand,
                                 IAsyncRequestHandler<ListTarefaQuery, ListTarefaResponse> listTarefaQuery,
                                 IAsyncRequestHandler<GetTarefaQuery, GetTarefaResponse> getTarefaQuery,
                                 IAsyncRequestHandler<GetByRecursoQuery, GetByRecursoResponse> getByRecursoQuery,
                                 IAsyncRequestHandler<RemoveTarefaCommand, RemoveTarefaResponse> removeTarefaCommand,
                                 IAsyncRequestHandler<UpdateTarefaCommand, UpdateTarefaResponse> updateTarefaCommand)
        {
            _createTarefaCommand = createTarefaCommand;
            _listTarefaQuery = listTarefaQuery;
            _getTarefaQuery = getTarefaQuery;
            _getByRecursoQuery = getByRecursoQuery;
            _removeTarefaCommand = removeTarefaCommand;
            _updateTarefaCommand = updateTarefaCommand;
        }

        public async UnaryResult<CreateTarefaResponse> AddAsync(CreateTarefaCommand command)
        {
            return await _createTarefaCommand.InvokeAsync(command);
        }

        public async UnaryResult<ListTarefaResponse> AllAsync(ListTarefaQuery query)
        {
            return await _listTarefaQuery.InvokeAsync(query);
        }

        public async UnaryResult<GetTarefaResponse> GetAsync(GetTarefaQuery query)
        {
            return await _getTarefaQuery.InvokeAsync(query);
        }

        public async UnaryResult<GetByRecursoResponse> GetByRecursoAsync(GetByRecursoQuery query)
        {
            return await _getByRecursoQuery.InvokeAsync(query);
        }

        public async UnaryResult<RemoveTarefaResponse> RemoveAsync(RemoveTarefaCommand command)
        {
            return await _removeTarefaCommand.InvokeAsync(command);
        }

        public async UnaryResult<UpdateTarefaResponse> UpdateAsync(UpdateTarefaCommand command)
        {
            return await _updateTarefaCommand.InvokeAsync(command);
        }
    }
}
