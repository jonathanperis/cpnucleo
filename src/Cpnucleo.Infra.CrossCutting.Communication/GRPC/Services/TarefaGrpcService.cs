using AutoMapper;
using Cpnucleo.Infra.CrossCutting.Communication.GRPC.Interfaces;
using Cpnucleo.Infra.CrossCutting.Communication.GRPC.Protos;
using Cpnucleo.Infra.CrossCutting.Communication.GRPC.Protos.Tarefa;
using Cpnucleo.Infra.CrossCutting.Util.Interfaces;
using Cpnucleo.Infra.CrossCutting.Util.ViewModels;
using Google.Protobuf.WellKnownTypes;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cpnucleo.Infra.CrossCutting.Communication.GRPC.Services
{
    internal class TarefaGrpcService : BaseGrpcService, ITarefaGrpcService
    {
        private readonly Tarefa.TarefaClient _client;

        public TarefaGrpcService(IMapper mapper, ISystemConfiguration systemConfiguration)
            : base(mapper, systemConfiguration)
        {
            _client = new Tarefa.TarefaClient(_channel);
        }

        public async Task<bool> IncluirAsync(TarefaViewModel obj)
        {
            BaseReply reply = await _client.IncluirAsync(_mapper.Map<TarefaModel>(obj));

            return reply.Sucesso;
        }

        public async Task<TarefaViewModel> ConsultarAsync(Guid id)
        {
            BaseRequest request = new BaseRequest
            {
                Id = id.ToString()
            };

            return _mapper.Map<TarefaViewModel>(await _client.ConsultarAsync(request));
        }

        public async Task<IEnumerable<TarefaViewModel>> ListarAsync()
        {
            ListarReply response = await _client.ListarAsync(new Empty());
            return _mapper.Map<IEnumerable<TarefaViewModel>>(response.Lista);
        }

        public async Task<bool> AlterarAsync(TarefaViewModel obj)
        {
            BaseReply reply = await _client.AlterarAsync(_mapper.Map<TarefaModel>(obj));

            return reply.Sucesso;
        }

        public async Task<bool> RemoverAsync(Guid id)
        {
            BaseRequest request = new BaseRequest
            {
                Id = id.ToString()
            };

            BaseReply reply = await _client.RemoverAsync(request);

            return reply.Sucesso;
        }

        public async Task<IEnumerable<TarefaViewModel>> ListarPorRecursoAsync(Guid idRecurso)
        {
            BaseRequest request = new BaseRequest
            {
                Id = idRecurso.ToString()
            };

            ListarPorRecursoReply response = await _client.ListarPorRecursoAsync(request);
            return _mapper.Map<IEnumerable<TarefaViewModel>>(response.Lista);
        }

        public async Task<bool> AlterarPorWorkflowAsync(Guid idTarefa, Guid idWorkflow)
        {
            AlterarPorWorkflowRequest request = new AlterarPorWorkflowRequest
            {
                IdTarefa = idTarefa.ToString(),
                IdWorkflow = idWorkflow.ToString()
            };

            BaseReply reply = await _client.AlterarPorWorkflowAsync(request);

            return reply.Sucesso;
        }
    }
}
