using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Threading.Tasks;
using ProtoBuf.Grpc.Client;
using Cpnucleo.Infra.CrossCutting.Util;
using Cpnucleo.Infra.CrossCutting.Util.ViewModels;
using Cpnucleo.RazorPages.Services.Interfaces;
using Grpc.Net.Client;

namespace Cpnucleo.RazorPages.Services
{
    internal class SistemaService : GrpcService, ICrudService<SistemaViewModel>
    {
        private ISistemaGrpcService _client;
        
        public SistemaService(IConfiguration configuration)
            : base(configuration)
        {

        }

        public async Task<IEnumerable<SistemaViewModel>> AllAsync(string token, string getDependencies, SistemaViewModel viewModel)
        {
            _client = InitializeAuthenticatedChannel(token);

            var lista = new List<SistemaViewModel>();
            lista.Add(new SistemaViewModel { Id = System.Guid.NewGuid(), Nome = "Teste 1", Descricao = "Descrição 1", DataInclusao = System.DateTime.Now, Sucesso = true, Workflow = new WorkflowViewModel { Id = System.Guid.NewGuid(), Nome = "Teste Workflow" } });
            lista.Add(new SistemaViewModel { Id = System.Guid.NewGuid(), Nome = "Teste 2", Descricao = "Descrição 2", DataInclusao = System.DateTime.Now, Sucesso = false });
            lista.Add(new SistemaViewModel { Id = System.Guid.NewGuid(), Nome = "Teste 3", Descricao = "Descrição 3", DataInclusao = System.DateTime.Now, Sucesso = true });
            lista.Add(new SistemaViewModel { Id = System.Guid.NewGuid(), Descricao = "Descrição 4", DataInclusao = System.DateTime.Now, Teste2 = "Teste 2" });

            return await _client.AllAsync(lista);
        }

        private ISistemaGrpcService InitializeAuthenticatedChannel(string token)
        {
            _channel = CreateAuthenticatedChannel(token);
            //_channel = GrpcChannel.ForAddress("https://localhost:5021");
            return _channel.CreateGrpcService<ISistemaGrpcService>();
        }
    }
}