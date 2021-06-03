using Cpnucleo.Infra.CrossCutting.Util.Commands.Requests.Tarefa;
using Cpnucleo.Infra.CrossCutting.Util.Commands.Responses.Tarefa;
using Cpnucleo.Infra.CrossCutting.Util.Interfaces;
using Cpnucleo.Infra.CrossCutting.Util.Queries.Requests.Tarefa;
using Cpnucleo.Infra.CrossCutting.Util.Queries.Responses.Tarefa;
using Cpnucleo.MVC.Interfaces;
using Microsoft.Extensions.Configuration;
using ProtoBuf.Grpc.Client;
using System.Threading.Tasks;

namespace Cpnucleo.MVC.Services
{
    internal class TarefaService : GrpcService, ITarefaService
    {
        private ITarefaGrpcService _TarefaGrpcService;

        public TarefaService(IConfiguration configuration)
            : base(configuration)
        {

        }

        public async Task<CreateTarefaResponse> AddAsync(string token, CreateTarefaCommand command)
        {
            _TarefaGrpcService = InitializeAuthenticatedChannel(token);
            return await _TarefaGrpcService.AddAsync(command);
        }

        public async Task<ListTarefaResponse> AllAsync(string token, ListTarefaQuery query)
        {
            _TarefaGrpcService = InitializeAuthenticatedChannel(token);
            return await _TarefaGrpcService.AllAsync(query);
        }

        public async Task<GetTarefaResponse> GetAsync(string token, GetTarefaQuery query)
        {
            _TarefaGrpcService = InitializeAuthenticatedChannel(token);
            return await _TarefaGrpcService.GetAsync(query);
        }

        public async Task<GetByRecursoResponse> GetByRecursoAsync(string token, GetByRecursoQuery query)
        {
            _TarefaGrpcService = InitializeAuthenticatedChannel(token);
            return await _TarefaGrpcService.GetByRecursoAsync(query);
        }

        public async Task<RemoveTarefaResponse> RemoveAsync(string token, RemoveTarefaCommand command)
        {
            _TarefaGrpcService = InitializeAuthenticatedChannel(token);
            return await _TarefaGrpcService.RemoveAsync(command);
        }

        public async Task<UpdateTarefaResponse> UpdateAsync(string token, UpdateTarefaCommand command)
        {
            _TarefaGrpcService = InitializeAuthenticatedChannel(token);
            return await _TarefaGrpcService.UpdateAsync(command);
        }

        private ITarefaGrpcService InitializeAuthenticatedChannel(string token)
        {
            _channel = CreateAuthenticatedChannel(token);
            return _channel.CreateGrpcService<ITarefaGrpcService>();
        }
    }
}