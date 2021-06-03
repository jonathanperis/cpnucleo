using Cpnucleo.Infra.CrossCutting.Util.Commands.Requests.Recurso;
using Cpnucleo.Infra.CrossCutting.Util.Commands.Responses.Recurso;
using Cpnucleo.Infra.CrossCutting.Util.Interfaces;
using Cpnucleo.Infra.CrossCutting.Util.Queries.Requests.Recurso;
using Cpnucleo.Infra.CrossCutting.Util.Queries.Responses.Recurso;
using Cpnucleo.MVC.Interfaces;
using Microsoft.Extensions.Configuration;
using ProtoBuf.Grpc.Client;
using System.Threading.Tasks;

namespace Cpnucleo.MVC.Services
{
    internal class RecursoService : GrpcService, IRecursoService
    {
        private IRecursoGrpcService _RecursoGrpcService;

        public RecursoService(IConfiguration configuration)
            : base(configuration)
        {

        }

        public async Task<CreateRecursoResponse> AddAsync(string token, CreateRecursoCommand command)
        {
            _RecursoGrpcService = InitializeAuthenticatedChannel(token);
            return await _RecursoGrpcService.AddAsync(command);
        }

        public async Task<ListRecursoResponse> AllAsync(string token, ListRecursoQuery query)
        {
            _RecursoGrpcService = InitializeAuthenticatedChannel(token);
            return await _RecursoGrpcService.AllAsync(query);
        }

        public async Task<AuthResponse> AuthAsync(AuthQuery query)
        {
            _RecursoGrpcService = InitializeAuthenticatedChannel(string.Empty);
            return await _RecursoGrpcService.AuthAsync(query);
        }

        public async Task<GetRecursoResponse> GetAsync(string token, GetRecursoQuery query)
        {
            _RecursoGrpcService = InitializeAuthenticatedChannel(token);
            return await _RecursoGrpcService.GetAsync(query);
        }

        public async Task<RemoveRecursoResponse> RemoveAsync(string token, RemoveRecursoCommand command)
        {
            _RecursoGrpcService = InitializeAuthenticatedChannel(token);
            return await _RecursoGrpcService.RemoveAsync(command);
        }

        public async Task<UpdateRecursoResponse> UpdateAsync(string token, UpdateRecursoCommand command)
        {
            _RecursoGrpcService = InitializeAuthenticatedChannel(token);
            return await _RecursoGrpcService.UpdateAsync(command);
        }

        private IRecursoGrpcService InitializeAuthenticatedChannel(string token)
        {
            _channel = CreateAuthenticatedChannel(token);
            return _channel.CreateGrpcService<IRecursoGrpcService>();
        }
    }
}