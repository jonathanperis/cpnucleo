using Cpnucleo.Infra.CrossCutting.Util.Commands.Requests.ImpedimentoTarefa;
using Cpnucleo.Infra.CrossCutting.Util.Commands.Responses.ImpedimentoTarefa;
using Cpnucleo.Infra.CrossCutting.Util.Interfaces;
using Cpnucleo.Infra.CrossCutting.Util.Queries.Requests.ImpedimentoTarefa;
using Cpnucleo.Infra.CrossCutting.Util.Queries.Responses.ImpedimentoTarefa;
using Cpnucleo.MVC.Interfaces;
using Microsoft.Extensions.Configuration;
using ProtoBuf.Grpc.Client;
using System.Threading.Tasks;

namespace Cpnucleo.MVC.Services
{
    internal class ImpedimentoTarefaService : GrpcService, IImpedimentoTarefaService
    {
        private IImpedimentoTarefaGrpcService _ImpedimentoTarefaGrpcService;

        public ImpedimentoTarefaService(IConfiguration configuration)
            : base(configuration)
        {

        }

        public async Task<CreateImpedimentoTarefaResponse> AddAsync(string token, CreateImpedimentoTarefaCommand command)
        {
            _ImpedimentoTarefaGrpcService = InitializeAuthenticatedChannel(token);
            return await _ImpedimentoTarefaGrpcService.AddAsync(command);
        }

        public async Task<ListImpedimentoTarefaResponse> AllAsync(string token, ListImpedimentoTarefaQuery query)
        {
            _ImpedimentoTarefaGrpcService = InitializeAuthenticatedChannel(token);
            return await _ImpedimentoTarefaGrpcService.AllAsync(query);
        }

        public async Task<GetImpedimentoTarefaResponse> GetAsync(string token, GetImpedimentoTarefaQuery query)
        {
            _ImpedimentoTarefaGrpcService = InitializeAuthenticatedChannel(token);
            return await _ImpedimentoTarefaGrpcService.GetAsync(query);
        }

        public async Task<GetByTarefaResponse> GetByTarefaAsync(string token, GetByTarefaQuery query)
        {
            _ImpedimentoTarefaGrpcService = InitializeAuthenticatedChannel(token);
            return await _ImpedimentoTarefaGrpcService.GetByTarefaAsync(query);
        }

        public async Task<RemoveImpedimentoTarefaResponse> RemoveAsync(string token, RemoveImpedimentoTarefaCommand command)
        {
            _ImpedimentoTarefaGrpcService = InitializeAuthenticatedChannel(token);
            return await _ImpedimentoTarefaGrpcService.RemoveAsync(command);
        }

        public async Task<UpdateImpedimentoTarefaResponse> UpdateAsync(string token, UpdateImpedimentoTarefaCommand command)
        {
            _ImpedimentoTarefaGrpcService = InitializeAuthenticatedChannel(token);
            return await _ImpedimentoTarefaGrpcService.UpdateAsync(command);
        }

        private IImpedimentoTarefaGrpcService InitializeAuthenticatedChannel(string token)
        {
            _channel = CreateAuthenticatedChannel(token);
            return _channel.CreateGrpcService<IImpedimentoTarefaGrpcService>();
        }
    }
}