using Cpnucleo.Infra.CrossCutting.Util.Commands.TipoTarefa.CreateTipoTarefa;
using Cpnucleo.Infra.CrossCutting.Util.Commands.TipoTarefa.RemoveTipoTarefa;
using Cpnucleo.Infra.CrossCutting.Util.Commands.TipoTarefa.UpdateTipoTarefa;
using Cpnucleo.Infra.CrossCutting.Util.Interfaces;
using Cpnucleo.Infra.CrossCutting.Util.Queries.TipoTarefa.GetTipoTarefa;
using Cpnucleo.Infra.CrossCutting.Util.Queries.TipoTarefa.ListTipoTarefa;
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