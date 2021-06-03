using Cpnucleo.Infra.CrossCutting.Util.Commands.RecursoTarefa.CreateRecursoTarefa;
using Cpnucleo.Infra.CrossCutting.Util.Commands.RecursoTarefa.RemoveRecursoTarefa;
using Cpnucleo.Infra.CrossCutting.Util.Commands.RecursoTarefa.UpdateRecursoTarefa;
using Cpnucleo.Infra.CrossCutting.Util.Interfaces;
using Cpnucleo.Infra.CrossCutting.Util.Queries.RecursoTarefa.GetByTarefa;
using Cpnucleo.Infra.CrossCutting.Util.Queries.RecursoTarefa.GetRecursoTarefa;
using Cpnucleo.Infra.CrossCutting.Util.Queries.RecursoTarefa.ListRecursoTarefa;
using Cpnucleo.MVC.Interfaces;
using Microsoft.Extensions.Configuration;
using ProtoBuf.Grpc.Client;
using System.Threading.Tasks;

namespace Cpnucleo.MVC.Services
{
    internal class RecursoTarefaService : GrpcService, IRecursoTarefaService
    {
        private IRecursoTarefaGrpcService _RecursoTarefaGrpcService;

        public RecursoTarefaService(IConfiguration configuration)
            : base(configuration)
        {

        }

        public async Task<CreateRecursoTarefaResponse> AddAsync(string token, CreateRecursoTarefaCommand command)
        {
            _RecursoTarefaGrpcService = InitializeAuthenticatedChannel(token);
            return await _RecursoTarefaGrpcService.AddAsync(command);
        }

        public async Task<ListRecursoTarefaResponse> AllAsync(string token, ListRecursoTarefaQuery query)
        {
            _RecursoTarefaGrpcService = InitializeAuthenticatedChannel(token);
            return await _RecursoTarefaGrpcService.AllAsync(query);
        }

        public async Task<GetRecursoTarefaResponse> GetAsync(string token, GetRecursoTarefaQuery query)
        {
            _RecursoTarefaGrpcService = InitializeAuthenticatedChannel(token);
            return await _RecursoTarefaGrpcService.GetAsync(query);
        }

        public async Task<GetByTarefaResponse> GetByTarefaAsync(string token, GetByTarefaQuery query)
        {
            _RecursoTarefaGrpcService = InitializeAuthenticatedChannel(token);
            return await _RecursoTarefaGrpcService.GetByTarefaAsync(query);
        }

        public async Task<RemoveRecursoTarefaResponse> RemoveAsync(string token, RemoveRecursoTarefaCommand command)
        {
            _RecursoTarefaGrpcService = InitializeAuthenticatedChannel(token);
            return await _RecursoTarefaGrpcService.RemoveAsync(command);
        }

        public async Task<UpdateRecursoTarefaResponse> UpdateAsync(string token, UpdateRecursoTarefaCommand command)
        {
            _RecursoTarefaGrpcService = InitializeAuthenticatedChannel(token);
            return await _RecursoTarefaGrpcService.UpdateAsync(command);
        }

        private IRecursoTarefaGrpcService InitializeAuthenticatedChannel(string token)
        {
            _channel = CreateAuthenticatedChannel(token);
            return _channel.CreateGrpcService<IRecursoTarefaGrpcService>();
        }
    }
}