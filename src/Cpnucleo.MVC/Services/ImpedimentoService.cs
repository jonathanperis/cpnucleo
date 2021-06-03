using Cpnucleo.Infra.CrossCutting.Util.Commands.Impedimento.CreateImpedimento;
using Cpnucleo.Infra.CrossCutting.Util.Commands.Impedimento.RemoveImpedimento;
using Cpnucleo.Infra.CrossCutting.Util.Commands.Impedimento.UpdateImpedimento;
using Cpnucleo.Infra.CrossCutting.Util.Interfaces;
using Cpnucleo.Infra.CrossCutting.Util.Queries.Impedimento.GetImpedimento;
using Cpnucleo.Infra.CrossCutting.Util.Queries.Impedimento.ListImpedimento;
using Cpnucleo.MVC.Interfaces;
using Microsoft.Extensions.Configuration;
using ProtoBuf.Grpc.Client;
using System.Threading.Tasks;

namespace Cpnucleo.MVC.Services
{
    internal class ImpedimentoService : GrpcService, IImpedimentoService
    {
        private IImpedimentoGrpcService _ImpedimentoGrpcService;

        public ImpedimentoService(IConfiguration configuration)
            : base(configuration)
        {

        }

        public async Task<CreateImpedimentoResponse> AddAsync(string token, CreateImpedimentoCommand command)
        {
            _ImpedimentoGrpcService = InitializeAuthenticatedChannel(token);
            return await _ImpedimentoGrpcService.AddAsync(command);
        }

        public async Task<ListImpedimentoResponse> AllAsync(string token, ListImpedimentoQuery query)
        {
            _ImpedimentoGrpcService = InitializeAuthenticatedChannel(token);
            return await _ImpedimentoGrpcService.AllAsync(query);
        }

        public async Task<GetImpedimentoResponse> GetAsync(string token, GetImpedimentoQuery query)
        {
            _ImpedimentoGrpcService = InitializeAuthenticatedChannel(token);
            return await _ImpedimentoGrpcService.GetAsync(query);
        }

        public async Task<RemoveImpedimentoResponse> RemoveAsync(string token, RemoveImpedimentoCommand command)
        {
            _ImpedimentoGrpcService = InitializeAuthenticatedChannel(token);
            return await _ImpedimentoGrpcService.RemoveAsync(command);
        }

        public async Task<UpdateImpedimentoResponse> UpdateAsync(string token, UpdateImpedimentoCommand command)
        {
            _ImpedimentoGrpcService = InitializeAuthenticatedChannel(token);
            return await _ImpedimentoGrpcService.UpdateAsync(command);
        }

        private IImpedimentoGrpcService InitializeAuthenticatedChannel(string token)
        {
            _channel = CreateAuthenticatedChannel(token);
            return _channel.CreateGrpcService<IImpedimentoGrpcService>();
        }
    }
}