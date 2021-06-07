using Cpnucleo.Infra.CrossCutting.Util.Commands.Projeto.CreateProjeto;
using Cpnucleo.Infra.CrossCutting.Util.Commands.Projeto.RemoveProjeto;
using Cpnucleo.Infra.CrossCutting.Util.Commands.Projeto.UpdateProjeto;
using Cpnucleo.Infra.CrossCutting.Util.Interfaces;
using Cpnucleo.Infra.CrossCutting.Util.Queries.Projeto.GetProjeto;
using Cpnucleo.Infra.CrossCutting.Util.Queries.Projeto.ListProjeto;
using MagicOnion;
using MagicOnion.Server;
using MessagePipe;
using Microsoft.AspNetCore.Authorization;

namespace Cpnucleo.GRPC.Services
{
    [Authorize]
    public class ProjetoGrpcService : ServiceBase<IProjetoGrpcService>, IProjetoGrpcService
    {
        private readonly IAsyncRequestHandler<CreateProjetoCommand, CreateProjetoResponse> _createProjetoCommand;
        private readonly IAsyncRequestHandler<ListProjetoQuery, ListProjetoResponse> _listProjetoQuery;
        private readonly IAsyncRequestHandler<GetProjetoQuery, GetProjetoResponse> _getProjetoQuery;
        private readonly IAsyncRequestHandler<RemoveProjetoCommand, RemoveProjetoResponse> _removeProjetoCommand;
        private readonly IAsyncRequestHandler<UpdateProjetoCommand, UpdateProjetoResponse> _updateProjetoCommand;

        public ProjetoGrpcService(IAsyncRequestHandler<CreateProjetoCommand, CreateProjetoResponse> createProjetoCommand,
                                  IAsyncRequestHandler<ListProjetoQuery, ListProjetoResponse> listProjetoQuery,
                                  IAsyncRequestHandler<GetProjetoQuery, GetProjetoResponse> getProjetoQuery,
                                  IAsyncRequestHandler<RemoveProjetoCommand, RemoveProjetoResponse> removeProjetoCommand,
                                  IAsyncRequestHandler<UpdateProjetoCommand, UpdateProjetoResponse> updateProjetoCommand)
        {
            _createProjetoCommand = createProjetoCommand;
            _listProjetoQuery = listProjetoQuery;
            _getProjetoQuery = getProjetoQuery;
            _removeProjetoCommand = removeProjetoCommand;
            _updateProjetoCommand = updateProjetoCommand;
        }

        public async UnaryResult<CreateProjetoResponse> AddAsync(CreateProjetoCommand command)
        {
            return await _createProjetoCommand.InvokeAsync(command);
        }

        public async UnaryResult<ListProjetoResponse> AllAsync(ListProjetoQuery query)
        {
            return await _listProjetoQuery.InvokeAsync(query);
        }

        public async UnaryResult<GetProjetoResponse> GetAsync(GetProjetoQuery query)
        {
            return await _getProjetoQuery.InvokeAsync(query);
        }

        public async UnaryResult<RemoveProjetoResponse> RemoveAsync(RemoveProjetoCommand command)
        {
            return await _removeProjetoCommand.InvokeAsync(command);
        }

        public async UnaryResult<UpdateProjetoResponse> UpdateAsync(UpdateProjetoCommand command)
        {
            return await _updateProjetoCommand.InvokeAsync(command);
        }
    }
}
