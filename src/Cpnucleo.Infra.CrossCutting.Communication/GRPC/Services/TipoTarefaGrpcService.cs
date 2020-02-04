using AutoMapper;
using Cpnucleo.Infra.CrossCutting.Communication.GRPC.Interfaces;
using Cpnucleo.Infra.CrossCutting.Communication.GRPC.Protos;
using Cpnucleo.Infra.CrossCutting.Util.ViewModels;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cpnucleo.Infra.CrossCutting.Communication.GRPC.Services
{
    public class TarefaGrpcService : BaseGrpcService, ITarefaGrpcService
    {
        private readonly Tarefa.TarefaClient _client;

        public TarefaGrpcService(IMapper mapper)
            : base(mapper)
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
            List<TarefaViewModel> result = new List<TarefaViewModel>();

            //using (var reply = _client.Listar(new ListarRequest()))
            //{
            //    await foreach (var item in reply.ResponseStream.ReadAllAsync())
            //    {
            //        result.Add(_mapper.Map<TarefaViewModel>(item));
            //    }
            //}

            using (AsyncServerStreamingCall<TarefaModel> reply = _client.Listar(new Empty()))
            {
                while (await reply.ResponseStream.MoveNext())
                {
                    result.Add(_mapper.Map<TarefaViewModel>(reply.ResponseStream.Current));
                }
            }

            return result;
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

        public Task<bool> AlterarPorPercentualConcluidoAsync(Guid idTarefa, int? percentualConcluido)
        {
            throw new NotImplementedException();
        }

        public Task<bool> AlterarPorWorkflowAsync(Guid idTarefa, Guid idWorkflow)
        {
            throw new NotImplementedException();
        }
    }
}
