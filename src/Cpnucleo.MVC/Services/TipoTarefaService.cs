using Cpnucleo.Infra.CrossCutting.Util.Commands.Requests.TipoTarefa;
using Cpnucleo.Infra.CrossCutting.Util.Commands.Responses.TipoTarefa;
using Cpnucleo.Infra.CrossCutting.Util.Interfaces;
using Cpnucleo.Infra.CrossCutting.Util.Queries.Requests.TipoTarefa;
using Cpnucleo.Infra.CrossCutting.Util.Queries.Responses.TipoTarefa;
using Cpnucleo.MVC.Interfaces;
using Microsoft.Extensions.Configuration;
using ProtoBuf.Grpc.Client;
using System.Threading.Tasks;

namespace Cpnucleo.MVC.Services
{
    internal class TipoTarefaService : GrpcService, ITipoTarefaService
    {
        private ITipoTarefaGrpcService _TipoTarefaGrpcService;

        public TipoTarefaService(IConfiguration configuration)
            : base(configuration)
        {

        }

        public async Task<CreateTipoTarefaResponse> AddAsync(string token, CreateTipoTarefaCommand command)
        {
            _TipoTarefaGrpcService = InitializeAuthenticatedChannel(token);
            return await _TipoTarefaGrpcService.AddAsync(command);
        }

        public async Task<ListTipoTarefaResponse> AllAsync(string token, ListTipoTarefaQuery query)
        {
            _TipoTarefaGrpcService = InitializeAuthenticatedChannel(token);
            return await _TipoTarefaGrpcService.AllAsync(query);
        }

        public async Task<GetTipoTarefaResponse> GetAsync(string token, GetTipoTarefaQuery query)
        {
            _TipoTarefaGrpcService = InitializeAuthenticatedChannel(token);
            return await _TipoTarefaGrpcService.GetAsync(query);
        }

        public async Task<RemoveTipoTarefaResponse> RemoveAsync(string token, RemoveTipoTarefaCommand command)
        {
            _TipoTarefaGrpcService = InitializeAuthenticatedChannel(token);
            return await _TipoTarefaGrpcService.RemoveAsync(command);
        }

        public async Task<UpdateTipoTarefaResponse> UpdateAsync(string token, UpdateTipoTarefaCommand command)
        {
            _TipoTarefaGrpcService = InitializeAuthenticatedChannel(token);
            return await _TipoTarefaGrpcService.UpdateAsync(command);
        }

        private ITipoTarefaGrpcService InitializeAuthenticatedChannel(string token)
        {
            _channel = CreateAuthenticatedChannel(token);
            return _channel.CreateGrpcService<ITipoTarefaGrpcService>();
        }
    }
}