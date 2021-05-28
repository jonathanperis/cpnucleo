using Cpnucleo.Infra.CrossCutting.Util.Commands.Requests.Sistema;
using Cpnucleo.Infra.CrossCutting.Util.Commands.Responses.Sistema;
using Cpnucleo.Infra.CrossCutting.Util.Interfaces;
using Cpnucleo.Infra.CrossCutting.Util.Queries.Requests.Sistema;
using Cpnucleo.Infra.CrossCutting.Util.Queries.Responses.Sistema;
using Cpnucleo.RazorPages.Interfaces;
using Microsoft.Extensions.Configuration;
using ProtoBuf.Grpc.Client;
using System.Threading.Tasks;

namespace Cpnucleo.MVC.Services
{
    internal class SistemaService : GrpcService, ISistemaService
    {
        private ISistemaGrpcService _sistemaGrpcService;

        public SistemaService(IConfiguration configuration)
            : base(configuration)
        {

        }

        public async Task<CreateSistemaResponse> AddAsync(string token, CreateSistemaCommand command)
        {
            _sistemaGrpcService = InitializeAuthenticatedChannel(token);
            return await _sistemaGrpcService.AddAsync(command);
        }

        public async Task<ListSistemaResponse> AllAsync(string token, ListSistemaQuery query)
        {
            _sistemaGrpcService = InitializeAuthenticatedChannel(token);
            return await _sistemaGrpcService.AllAsync(query);
        }

        public async Task<GetSistemaResponse> GetAsync(string token, GetSistemaQuery query)
        {
            _sistemaGrpcService = InitializeAuthenticatedChannel(token);
            return await _sistemaGrpcService.GetAsync(query);
        }

        public async Task<RemoveSistemaResponse> RemoveAsync(string token, RemoveSistemaCommand command)
        {
            _sistemaGrpcService = InitializeAuthenticatedChannel(token);
            return await _sistemaGrpcService.RemoveAsync(command);
        }

        public async Task<UpdateSistemaResponse> UpdateAsync(string token, UpdateSistemaCommand command)
        {
            _sistemaGrpcService = InitializeAuthenticatedChannel(token);
            return await _sistemaGrpcService.UpdateAsync(command);
        }

        private ISistemaGrpcService InitializeAuthenticatedChannel(string token)
        {
            _channel = CreateAuthenticatedChannel(token);
            return _channel.CreateGrpcService<ISistemaGrpcService>();
        }
    }
}