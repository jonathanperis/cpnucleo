using AutoMapper;
using Cpnucleo.Infra.CrossCutting.Communication.GRPC.Interfaces;
using Cpnucleo.Infra.CrossCutting.Communication.GRPC.Protos;
using Cpnucleo.Infra.CrossCutting.Communication.GRPC.Protos.ImpedimentoTarefa;
using Cpnucleo.Infra.CrossCutting.Util.Interfaces;
using Cpnucleo.Infra.CrossCutting.Util.ViewModels;
using Google.Protobuf.WellKnownTypes;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cpnucleo.Infra.CrossCutting.Communication.GRPC.Services
{
    internal class ImpedimentoTarefaGrpcService : BaseGrpcService, IImpedimentoTarefaGrpcService
    {
        private readonly ImpedimentoTarefa.ImpedimentoTarefaClient _client;

        public ImpedimentoTarefaGrpcService(IMapper mapper, ISystemConfiguration systemConfiguration)
            : base(mapper, systemConfiguration)
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
            ListarReply response = await _client.ListarAsync(new Empty());
            return _mapper.Map<IEnumerable<ImpedimentoTarefaViewModel>>(response.Lista);
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

        public async Task<IEnumerable<ImpedimentoTarefaViewModel>> ListarPorTarefaAsync(Guid idTarefa)
        {
            BaseRequest request = new BaseRequest
            {
                Id = idTarefa.ToString()
            };

            ListarPorTarefaReply response = await _client.ListarPorTarefaAsync(request);
            return _mapper.Map<IEnumerable<ImpedimentoTarefaViewModel>>(response.Lista);
        }
    }
}
