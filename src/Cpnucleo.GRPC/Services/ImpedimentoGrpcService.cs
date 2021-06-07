using Cpnucleo.Infra.CrossCutting.Util.Commands.Impedimento.CreateImpedimento;
using Cpnucleo.Infra.CrossCutting.Util.Commands.Impedimento.RemoveImpedimento;
using Cpnucleo.Infra.CrossCutting.Util.Commands.Impedimento.UpdateImpedimento;
using Cpnucleo.Infra.CrossCutting.Util.Interfaces;
using Cpnucleo.Infra.CrossCutting.Util.Queries.Impedimento.GetImpedimento;
using Cpnucleo.Infra.CrossCutting.Util.Queries.Impedimento.ListImpedimento;
using MagicOnion;
using MagicOnion.Server;
using MessagePipe;
using Microsoft.AspNetCore.Authorization;

namespace Cpnucleo.GRPC.Services
{
    [Authorize]
    public class ImpedimentoGrpcService : ServiceBase<IImpedimentoGrpcService>, IImpedimentoGrpcService
    {
        private readonly IAsyncRequestHandler<CreateImpedimentoCommand, CreateImpedimentoResponse> _createImpedimentoCommand;
        private readonly IAsyncRequestHandler<ListImpedimentoQuery, ListImpedimentoResponse> _listImpedimentoQuery;
        private readonly IAsyncRequestHandler<GetImpedimentoQuery, GetImpedimentoResponse> _getImpedimentoQuery;
        private readonly IAsyncRequestHandler<RemoveImpedimentoCommand, RemoveImpedimentoResponse> _removeImpedimentoCommand;
        private readonly IAsyncRequestHandler<UpdateImpedimentoCommand, UpdateImpedimentoResponse> _updateImpedimentoCommand;

        public ImpedimentoGrpcService(IAsyncRequestHandler<CreateImpedimentoCommand, CreateImpedimentoResponse> createImpedimentoCommand,
                                      IAsyncRequestHandler<ListImpedimentoQuery, ListImpedimentoResponse> listImpedimentoQuery,
                                      IAsyncRequestHandler<GetImpedimentoQuery, GetImpedimentoResponse> getImpedimentoQuery,
                                      IAsyncRequestHandler<RemoveImpedimentoCommand, RemoveImpedimentoResponse> removeImpedimentoCommand,
                                      IAsyncRequestHandler<UpdateImpedimentoCommand, UpdateImpedimentoResponse> updateImpedimentoCommand)
        {
            _createImpedimentoCommand = createImpedimentoCommand;
            _listImpedimentoQuery = listImpedimentoQuery;
            _getImpedimentoQuery = getImpedimentoQuery;
            _removeImpedimentoCommand = removeImpedimentoCommand;
            _updateImpedimentoCommand = updateImpedimentoCommand;
        }

        public async UnaryResult<CreateImpedimentoResponse> AddAsync(CreateImpedimentoCommand command)
        {
            return await _createImpedimentoCommand.InvokeAsync(command);
        }

        public async UnaryResult<ListImpedimentoResponse> AllAsync(ListImpedimentoQuery query)
        {
            return await _listImpedimentoQuery.InvokeAsync(query);
        }

        public async UnaryResult<GetImpedimentoResponse> GetAsync(GetImpedimentoQuery query)
        {
            return await _getImpedimentoQuery.InvokeAsync(query);
        }

        public async UnaryResult<RemoveImpedimentoResponse> RemoveAsync(RemoveImpedimentoCommand command)
        {
            return await _removeImpedimentoCommand.InvokeAsync(command);
        }

        public async UnaryResult<UpdateImpedimentoResponse> UpdateAsync(UpdateImpedimentoCommand command)
        {
            return await _updateImpedimentoCommand.InvokeAsync(command);
        }
    }
}
