using Cpnucleo.Infra.CrossCutting.Util.Commands.Requests.RecursoProjeto;
using Cpnucleo.Infra.CrossCutting.Util.Commands.Responses.RecursoProjeto;
using Cpnucleo.Infra.CrossCutting.Util.Interfaces;
using Cpnucleo.Infra.CrossCutting.Util.Queries.Requests.RecursoProjeto;
using Cpnucleo.Infra.CrossCutting.Util.Queries.Responses.RecursoProjeto;
using Cpnucleo.MVC.Interfaces;
using Microsoft.Extensions.Configuration;
using ProtoBuf.Grpc.Client;
using System.Threading.Tasks;

namespace Cpnucleo.MVC.Services
{
    internal class RecursoProjetoService : GrpcService, IRecursoProjetoService
    {
        private IRecursoProjetoGrpcService _RecursoProjetoGrpcService;

        public RecursoProjetoService(IConfiguration configuration)
            : base(configuration)
        {

        }

        public async Task<CreateRecursoProjetoResponse> AddAsync(string token, CreateRecursoProjetoCommand command)
        {
            _RecursoProjetoGrpcService = InitializeAuthenticatedChannel(token);
            return await _RecursoProjetoGrpcService.AddAsync(command);
        }

        public async Task<ListRecursoProjetoResponse> AllAsync(string token, ListRecursoProjetoQuery query)
        {
            _RecursoProjetoGrpcService = InitializeAuthenticatedChannel(token);
            return await _RecursoProjetoGrpcService.AllAsync(query);
        }

        public async Task<GetRecursoProjetoResponse> GetAsync(string token, GetRecursoProjetoQuery query)
        {
            _RecursoProjetoGrpcService = InitializeAuthenticatedChannel(token);
            return await _RecursoProjetoGrpcService.GetAsync(query);
        }

        public async Task<GetByProjetoResponse> GetByProjetoAsync(string token, GetByProjetoQuery query)
        {
            _RecursoProjetoGrpcService = InitializeAuthenticatedChannel(token);
            return await _RecursoProjetoGrpcService.GetByProjetoAsync(query);
        }

        public async Task<RemoveRecursoProjetoResponse> RemoveAsync(string token, RemoveRecursoProjetoCommand command)
        {
            _RecursoProjetoGrpcService = InitializeAuthenticatedChannel(token);
            return await _RecursoProjetoGrpcService.RemoveAsync(command);
        }

        public async Task<UpdateRecursoProjetoResponse> UpdateAsync(string token, UpdateRecursoProjetoCommand command)
        {
            _RecursoProjetoGrpcService = InitializeAuthenticatedChannel(token);
            return await _RecursoProjetoGrpcService.UpdateAsync(command);
        }

        private IRecursoProjetoGrpcService InitializeAuthenticatedChannel(string token)
        {
            _channel = CreateAuthenticatedChannel(token);
            return _channel.CreateGrpcService<IRecursoProjetoGrpcService>();
        }
    }
}