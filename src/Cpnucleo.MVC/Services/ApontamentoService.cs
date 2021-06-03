using Cpnucleo.Infra.CrossCutting.Util.Commands.Apontamento.CreateApontamento;
using Cpnucleo.Infra.CrossCutting.Util.Commands.Apontamento.RemoveApontamento;
using Cpnucleo.Infra.CrossCutting.Util.Commands.Apontamento.UpdateApontamento;
using Cpnucleo.Infra.CrossCutting.Util.Interfaces;
using Cpnucleo.Infra.CrossCutting.Util.Queries.Apontamento.GetApontamento;
using Cpnucleo.Infra.CrossCutting.Util.Queries.Apontamento.GetByRecurso;
using Cpnucleo.Infra.CrossCutting.Util.Queries.Apontamento.GetTotalHorasPorRecurso;
using Cpnucleo.Infra.CrossCutting.Util.Queries.Apontamento.ListApontamento;
using Cpnucleo.MVC.Interfaces;
using Microsoft.Extensions.Configuration;
using ProtoBuf.Grpc.Client;
using System.Threading.Tasks;

namespace Cpnucleo.MVC.Services
{
    internal class ApontamentoService : GrpcService, IApontamentoService
    {
        private IApontamentoGrpcService _ApontamentoGrpcService;

        public ApontamentoService(IConfiguration configuration)
            : base(configuration)
        {

        }

        public async Task<CreateApontamentoResponse> AddAsync(string token, CreateApontamentoCommand command)
        {
            _ApontamentoGrpcService = InitializeAuthenticatedChannel(token);
            return await _ApontamentoGrpcService.AddAsync(command);
        }

        public async Task<ListApontamentoResponse> AllAsync(string token, ListApontamentoQuery query)
        {
            _ApontamentoGrpcService = InitializeAuthenticatedChannel(token);
            return await _ApontamentoGrpcService.AllAsync(query);
        }

        public async Task<GetApontamentoResponse> GetAsync(string token, GetApontamentoQuery query)
        {
            _ApontamentoGrpcService = InitializeAuthenticatedChannel(token);
            return await _ApontamentoGrpcService.GetAsync(query);
        }

        public async Task<GetByRecursoResponse> GetByRecursoAsync(string token, GetByRecursoQuery query)
        {
            _ApontamentoGrpcService = InitializeAuthenticatedChannel(token);
            return await _ApontamentoGrpcService.GetByRecursoAsync(query);
        }

        public async Task<GetTotalHorasPorRecursoResponse> GetTotalHorasPorRecursoAsync(string token, GetTotalHorasPorRecursoQuery query)
        {
            _ApontamentoGrpcService = InitializeAuthenticatedChannel(token);
            return await _ApontamentoGrpcService.GetTotalHorasPorRecursoAsync(query);
        }

        public async Task<RemoveApontamentoResponse> RemoveAsync(string token, RemoveApontamentoCommand command)
        {
            _ApontamentoGrpcService = InitializeAuthenticatedChannel(token);
            return await _ApontamentoGrpcService.RemoveAsync(command);
        }

        public async Task<UpdateApontamentoResponse> UpdateAsync(string token, UpdateApontamentoCommand command)
        {
            _ApontamentoGrpcService = InitializeAuthenticatedChannel(token);
            return await _ApontamentoGrpcService.UpdateAsync(command);
        }

        private IApontamentoGrpcService InitializeAuthenticatedChannel(string token)
        {
            _channel = CreateAuthenticatedChannel(token);
            return _channel.CreateGrpcService<IApontamentoGrpcService>();
        }
    }
}