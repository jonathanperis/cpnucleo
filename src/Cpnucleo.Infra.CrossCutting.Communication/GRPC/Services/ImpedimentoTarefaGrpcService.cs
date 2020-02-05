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
    public class ImpedimentoTarefaGrpcService : BaseGrpcService, IImpedimentoTarefaGrpcService
    {
        private readonly ImpedimentoTarefa.ImpedimentoTarefaClient _client;

        public ImpedimentoTarefaGrpcService(IMapper mapper)
            : base(mapper)
        {
            _client = new ImpedimentoTarefa.ImpedimentoTarefaClient(_channel);
        }

        public async Task<bool> IncluirAsync(ImpedimentoTarefaViewModel obj)
        {
            BaseReply reply = await _client.IncluirAsync(_mapper.Map<ImpedimentoTarefaModel>(obj));

            return reply.Sucesso;
        }

        public async Task<ImpedimentoTarefaViewModel> ConsultarAsync(Guid id)
        {
            BaseRequest request = new BaseRequest
            {
                Id = id.ToString()
            };

            return _mapper.Map<ImpedimentoTarefaViewModel>(await _client.ConsultarAsync(request));
        }

        public async Task<IEnumerable<ImpedimentoTarefaViewModel>> ListarAsync()
        {
            List<ImpedimentoTarefaViewModel> result = new List<ImpedimentoTarefaViewModel>();

            //using (var reply = _client.Listar(new ListarRequest()))
            //{
            //    await foreach (var item in reply.ResponseStream.ReadAllAsync())
            //    {
            //        result.Add(_mapper.Map<ImpedimentoTarefaViewModel>(item));
            //    }
            //}

            using (AsyncServerStreamingCall<ImpedimentoTarefaModel> reply = _client.Listar(new Empty()))
            {
                while (await reply.ResponseStream.MoveNext())
                {
                    result.Add(_mapper.Map<ImpedimentoTarefaViewModel>(reply.ResponseStream.Current));
                }
            }

            return result;
        }

        public async Task<bool> AlterarAsync(ImpedimentoTarefaViewModel obj)
        {
            BaseReply reply = await _client.AlterarAsync(_mapper.Map<ImpedimentoTarefaModel>(obj));

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

        public Task<IEnumerable<ImpedimentoTarefaViewModel>> ListarPorTarefaAsync(Guid idTarefa)
        {
            throw new NotImplementedException();
        }
    }
}
