using Cpnucleo.Infra.CrossCutting.Util;
using Cpnucleo.Infra.CrossCutting.Util.Commands.Recurso.CreateRecurso;
using Cpnucleo.Infra.CrossCutting.Util.Commands.Recurso.RemoveRecurso;
using Cpnucleo.Infra.CrossCutting.Util.Commands.Recurso.UpdateRecurso;
using Cpnucleo.Infra.CrossCutting.Util.Interfaces;
using Cpnucleo.Infra.CrossCutting.Util.Queries.Recurso.Auth;
using Cpnucleo.Infra.CrossCutting.Util.Queries.Recurso.GetRecurso;
using Cpnucleo.Infra.CrossCutting.Util.Queries.Recurso.ListRecurso;
using MagicOnion;
using MagicOnion.Server;
using MessagePipe;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;

namespace Cpnucleo.GRPC.Services
{
    public class RecursoGrpcService : ServiceBase<IRecursoGrpcService>, IRecursoGrpcService
    {
        private readonly IAsyncRequestHandler<CreateRecursoCommand, CreateRecursoResponse> _createRecursoCommand;
        private readonly IAsyncRequestHandler<ListRecursoQuery, ListRecursoResponse> _listRecursoQuery;
        private readonly IAsyncRequestHandler<AuthQuery, AuthResponse> _authQuery;
        private readonly IAsyncRequestHandler<GetRecursoQuery, GetRecursoResponse> _getRecursoQuery;
        private readonly IAsyncRequestHandler<RemoveRecursoCommand, RemoveRecursoResponse> _removeRecursoCommand;
        private readonly IAsyncRequestHandler<UpdateRecursoCommand, UpdateRecursoResponse> _updateRecursoCommand;
        private readonly IConfiguration _configuration;

        public RecursoGrpcService(IAsyncRequestHandler<CreateRecursoCommand, CreateRecursoResponse> createRecursoCommand,
                                  IAsyncRequestHandler<ListRecursoQuery, ListRecursoResponse> listRecursoQuery,
                                  IAsyncRequestHandler<AuthQuery, AuthResponse> authQuery,
                                  IAsyncRequestHandler<GetRecursoQuery, GetRecursoResponse> getRecursoQuery,
                                  IAsyncRequestHandler<RemoveRecursoCommand, RemoveRecursoResponse> removeRecursoCommand,
                                  IAsyncRequestHandler<UpdateRecursoCommand, UpdateRecursoResponse> updateRecursoCommand,
                                  IConfiguration configuration)
        {
            _createRecursoCommand = createRecursoCommand;
            _listRecursoQuery = listRecursoQuery;
            _authQuery = authQuery;
            _getRecursoQuery = getRecursoQuery;
            _removeRecursoCommand = removeRecursoCommand;
            _updateRecursoCommand = updateRecursoCommand;
            _configuration = configuration;
        }

        [Authorize]
        public async UnaryResult<CreateRecursoResponse> AddAsync(CreateRecursoCommand command)
        {
            return await _createRecursoCommand.InvokeAsync(command);
        }

        [Authorize]
        public async UnaryResult<ListRecursoResponse> AllAsync(ListRecursoQuery query)
        {
            return await _listRecursoQuery.InvokeAsync(query);
        }

        public async UnaryResult<AuthResponse> AuthAsync(AuthQuery query)
        {
            AuthResponse response = await _authQuery.InvokeAsync(query);

            if (response.Status == OperationResult.Success)
            {
                int.TryParse(_configuration["Jwt:Expires"], out int jwtExpires);

                response.Recurso.Token = TokenService.GenerateToken(response.Recurso.Id.ToString(), _configuration["Jwt:Key"], _configuration["Jwt:Issuer"], jwtExpires);
            }

            return response;
        }

        [Authorize]
        public async UnaryResult<GetRecursoResponse> GetAsync(GetRecursoQuery query)
        {
            return await _getRecursoQuery.InvokeAsync(query);
        }

        [Authorize]
        public async UnaryResult<RemoveRecursoResponse> RemoveAsync(RemoveRecursoCommand command)
        {
            return await _removeRecursoCommand.InvokeAsync(command);
        }

        [Authorize]
        public async UnaryResult<UpdateRecursoResponse> UpdateAsync(UpdateRecursoCommand command)
        {
            return await _updateRecursoCommand.InvokeAsync(command);
        }
    }
}
