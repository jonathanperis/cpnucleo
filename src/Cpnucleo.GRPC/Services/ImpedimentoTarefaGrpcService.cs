using Cpnucleo.Infra.CrossCutting.Util.Commands.ImpedimentoTarefa.CreateImpedimentoTarefa;
using Cpnucleo.Infra.CrossCutting.Util.Commands.ImpedimentoTarefa.RemoveImpedimentoTarefa;
using Cpnucleo.Infra.CrossCutting.Util.Commands.ImpedimentoTarefa.UpdateImpedimentoTarefa;
using Cpnucleo.Infra.CrossCutting.Util.Interfaces;
using Cpnucleo.Infra.CrossCutting.Util.Queries.ImpedimentoTarefa.GetByTarefa;
using Cpnucleo.Infra.CrossCutting.Util.Queries.ImpedimentoTarefa.GetImpedimentoTarefa;
using Cpnucleo.Infra.CrossCutting.Util.Queries.ImpedimentoTarefa.ListImpedimentoTarefa;
using MagicOnion;
using MagicOnion.Server;
using MessagePipe;
using Microsoft.AspNetCore.Authorization;

namespace Cpnucleo.GRPC.Services
{
    [Authorize]
    public class ImpedimentoTarefaGrpcService : ServiceBase<IImpedimentoTarefaGrpcService>, IImpedimentoTarefaGrpcService
    {
        private readonly IAsyncRequestHandler<CreateImpedimentoTarefaCommand, CreateImpedimentoTarefaResponse> _createImpedimentoTarefaCommand;
        private readonly IAsyncRequestHandler<ListImpedimentoTarefaQuery, ListImpedimentoTarefaResponse> _listImpedimentoTarefaQuery;
        private readonly IAsyncRequestHandler<GetImpedimentoTarefaQuery, GetImpedimentoTarefaResponse> _getImpedimentoTarefaQuery;
        private readonly IAsyncRequestHandler<GetByTarefaQuery, GetByTarefaResponse> _getByTarefaQuery;
        private readonly IAsyncRequestHandler<RemoveImpedimentoTarefaCommand, RemoveImpedimentoTarefaResponse> _removeImpedimentoTarefaCommand;
        private readonly IAsyncRequestHandler<UpdateImpedimentoTarefaCommand, UpdateImpedimentoTarefaResponse> _updateImpedimentoTarefaCommand;

        public ImpedimentoTarefaGrpcService(IAsyncRequestHandler<CreateImpedimentoTarefaCommand, CreateImpedimentoTarefaResponse> createImpedimentoTarefaCommand, 
                                            IAsyncRequestHandler<ListImpedimentoTarefaQuery, ListImpedimentoTarefaResponse> listImpedimentoTarefaQuery, 
                                            IAsyncRequestHandler<GetImpedimentoTarefaQuery, GetImpedimentoTarefaResponse> getImpedimentoTarefaQuery, 
                                            IAsyncRequestHandler<GetByTarefaQuery, GetByTarefaResponse> getByTarefaQuery, 
                                            IAsyncRequestHandler<RemoveImpedimentoTarefaCommand, RemoveImpedimentoTarefaResponse> removeImpedimentoTarefaCommand, 
                                            IAsyncRequestHandler<UpdateImpedimentoTarefaCommand, UpdateImpedimentoTarefaResponse> updateImpedimentoTarefaCommand)
        {
            _createImpedimentoTarefaCommand = createImpedimentoTarefaCommand;
            _listImpedimentoTarefaQuery = listImpedimentoTarefaQuery;
            _getImpedimentoTarefaQuery = getImpedimentoTarefaQuery;
            _getByTarefaQuery = getByTarefaQuery;
            _removeImpedimentoTarefaCommand = removeImpedimentoTarefaCommand;
            _updateImpedimentoTarefaCommand = updateImpedimentoTarefaCommand;
        }

        public async UnaryResult<CreateImpedimentoTarefaResponse> AddAsync(CreateImpedimentoTarefaCommand command)
        {
            return await _createImpedimentoTarefaCommand.InvokeAsync(command);
        }

        public async UnaryResult<ListImpedimentoTarefaResponse> AllAsync(ListImpedimentoTarefaQuery query)
        {
            return await _listImpedimentoTarefaQuery.InvokeAsync(query);
        }

        public async UnaryResult<GetImpedimentoTarefaResponse> GetAsync(GetImpedimentoTarefaQuery query)
        {
            return await _getImpedimentoTarefaQuery.InvokeAsync(query);
        }

        public async UnaryResult<GetByTarefaResponse> GetByTarefaAsync(GetByTarefaQuery query)
        {
            return await _getByTarefaQuery.InvokeAsync(query);
        }

        public async UnaryResult<RemoveImpedimentoTarefaResponse> RemoveAsync(RemoveImpedimentoTarefaCommand command)
        {
            return await _removeImpedimentoTarefaCommand.InvokeAsync(command);
        }

        public async UnaryResult<UpdateImpedimentoTarefaResponse> UpdateAsync(UpdateImpedimentoTarefaCommand command)
        {
            return await _updateImpedimentoTarefaCommand.InvokeAsync(command);
        }
    }
}
