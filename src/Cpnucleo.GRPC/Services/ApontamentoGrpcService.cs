using Cpnucleo.Infra.CrossCutting.Util.Commands.Apontamento.CreateApontamento;
using Cpnucleo.Infra.CrossCutting.Util.Commands.Apontamento.RemoveApontamento;
using Cpnucleo.Infra.CrossCutting.Util.Commands.Apontamento.UpdateApontamento;
using Cpnucleo.Infra.CrossCutting.Util.Interfaces;
using Cpnucleo.Infra.CrossCutting.Util.Queries.Apontamento.GetApontamento;
using Cpnucleo.Infra.CrossCutting.Util.Queries.Apontamento.GetByRecurso;
using Cpnucleo.Infra.CrossCutting.Util.Queries.Apontamento.ListApontamento;
using MagicOnion;
using MagicOnion.Server;
using MessagePipe;
using Microsoft.AspNetCore.Authorization;

namespace Cpnucleo.GRPC.Services
{
    [Authorize]
    public class ApontamentoGrpcService : ServiceBase<IApontamentoGrpcService>, IApontamentoGrpcService
    {
        private readonly IAsyncRequestHandler<CreateApontamentoCommand, CreateApontamentoResponse> _createApontamentoCommand;
        private readonly IAsyncRequestHandler<ListApontamentoQuery, ListApontamentoResponse> _listApontamentoQuery;
        private readonly IAsyncRequestHandler<GetApontamentoQuery, GetApontamentoResponse> _getApontamentoQuery;
        private readonly IAsyncRequestHandler<GetByRecursoQuery, GetByRecursoResponse> _getByRecursoQuery;
        private readonly IAsyncRequestHandler<RemoveApontamentoCommand, RemoveApontamentoResponse> _removeApontamentoCommand;
        private readonly IAsyncRequestHandler<UpdateApontamentoCommand, UpdateApontamentoResponse> _updateApontamentoCommand;

        public ApontamentoGrpcService(IAsyncRequestHandler<CreateApontamentoCommand, CreateApontamentoResponse> createApontamentoCommand, 
                                      IAsyncRequestHandler<ListApontamentoQuery, ListApontamentoResponse> listApontamentoQuery, 
                                      IAsyncRequestHandler<GetApontamentoQuery, GetApontamentoResponse> getApontamentoQuery, 
                                      IAsyncRequestHandler<GetByRecursoQuery, GetByRecursoResponse> getByRecursoQuery, 
                                      IAsyncRequestHandler<RemoveApontamentoCommand, RemoveApontamentoResponse> removeApontamentoCommand, 
                                      IAsyncRequestHandler<UpdateApontamentoCommand, UpdateApontamentoResponse> updateApontamentoCommand)
        {
            _createApontamentoCommand = createApontamentoCommand;
            _listApontamentoQuery = listApontamentoQuery;
            _getApontamentoQuery = getApontamentoQuery;
            _getByRecursoQuery = getByRecursoQuery;
            _removeApontamentoCommand = removeApontamentoCommand;
            _updateApontamentoCommand = updateApontamentoCommand;
        }

        public async UnaryResult<CreateApontamentoResponse> AddAsync(CreateApontamentoCommand command)
        {
            return await _createApontamentoCommand.InvokeAsync(command);
        }

        public async UnaryResult<ListApontamentoResponse> AllAsync(ListApontamentoQuery query)
        {
            return await _listApontamentoQuery.InvokeAsync(query);
        }

        public async UnaryResult<GetApontamentoResponse> GetAsync(GetApontamentoQuery query)
        {
            return await _getApontamentoQuery.InvokeAsync(query);
        }

        public async UnaryResult<GetByRecursoResponse> GetByRecursoAsync(GetByRecursoQuery query)
        {
            return await _getByRecursoQuery.InvokeAsync(query);
        }

        public async UnaryResult<RemoveApontamentoResponse> RemoveAsync(RemoveApontamentoCommand command)
        {
            return await _removeApontamentoCommand.InvokeAsync(command);
        }

        public async UnaryResult<UpdateApontamentoResponse> UpdateAsync(UpdateApontamentoCommand command)
        {
            return await _updateApontamentoCommand.InvokeAsync(command);
        }
    }
}
