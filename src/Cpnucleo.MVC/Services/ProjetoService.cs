using Cpnucleo.Infra.CrossCutting.Util.Commands.Projeto.CreateProjeto;
using Cpnucleo.Infra.CrossCutting.Util.Commands.Projeto.RemoveProjeto;
using Cpnucleo.Infra.CrossCutting.Util.Commands.Projeto.UpdateProjeto;
using Cpnucleo.Infra.CrossCutting.Util.Interfaces;
using Cpnucleo.Infra.CrossCutting.Util.Queries.Projeto.GetProjeto;
using Cpnucleo.Infra.CrossCutting.Util.Queries.Projeto.ListProjeto;
using Cpnucleo.MVC.Interfaces;
using Microsoft.Extensions.Configuration;
using ProtoBuf.Grpc.Client;
using System.Threading.Tasks;

namespace Cpnucleo.MVC.Services
{
    internal class ProjetoService : GrpcService, IProjetoService
    {
        private IProjetoGrpcService _ProjetoGrpcService;

        public ProjetoService(IConfiguration configuration)
            : base(configuration)
        {

        }

        public async Task<CreateProjetoResponse> AddAsync(string token, CreateProjetoCommand command)
        {
            _ProjetoGrpcService = InitializeAuthenticatedChannel(token);
            return await _ProjetoGrpcService.AddAsync(command);
        }

        public async Task<ListProjetoResponse> AllAsync(string token, ListProjetoQuery query)
        {
            _ProjetoGrpcService = InitializeAuthenticatedChannel(token);
            return await _ProjetoGrpcService.AllAsync(query);
        }

        public async Task<GetProjetoResponse> GetAsync(string token, GetProjetoQuery query)
        {
            _ProjetoGrpcService = InitializeAuthenticatedChannel(token);
            return await _ProjetoGrpcService.GetAsync(query);
        }

        public async Task<RemoveProjetoResponse> RemoveAsync(string token, RemoveProjetoCommand command)
        {
            _ProjetoGrpcService = InitializeAuthenticatedChannel(token);
            return await _ProjetoGrpcService.RemoveAsync(command);
        }

        public async Task<UpdateProjetoResponse> UpdateAsync(string token, UpdateProjetoCommand command)
        {
            _ProjetoGrpcService = InitializeAuthenticatedChannel(token);
            return await _ProjetoGrpcService.UpdateAsync(command);
        }

        private IProjetoGrpcService InitializeAuthenticatedChannel(string token)
        {
            _channel = CreateAuthenticatedChannel(token);
            return _channel.CreateGrpcService<IProjetoGrpcService>();
        }
    }
}