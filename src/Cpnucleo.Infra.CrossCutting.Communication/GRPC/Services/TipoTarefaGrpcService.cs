using AutoMapper;
using Cpnucleo.Infra.CrossCutting.Communication.GRPC.Interfaces;
using Cpnucleo.Infra.CrossCutting.Communication.GRPC.Protos;
using Cpnucleo.Infra.CrossCutting.Communication.GRPC.Protos.TipoTarefa;
using Cpnucleo.Infra.CrossCutting.Util.Interfaces;
using Cpnucleo.Infra.CrossCutting.Util.ViewModels;
using Google.Protobuf.WellKnownTypes;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cpnucleo.Infra.CrossCutting.Communication.GRPC.Services
{
    internal class TipoTarefaGrpcService : BaseGrpcService, ICrudGrpcService<TipoTarefaViewModel>
    {
        private readonly TipoTarefa.TipoTarefaClient _client;

        public TipoTarefaGrpcService(IMapper mapper, ISystemConfiguration systemConfiguration)
            : base(mapper, systemConfiguration)
        {
            _client = new TipoTarefa.TipoTarefaClient(_channel);
        }

        public async Task<bool> IncluirAsync(TipoTarefaViewModel obj)
        {
            BaseReply reply = await _client.IncluirAsync(_mapper.Map<TipoTarefaModel>(obj));

            return reply.Sucesso;
        }

        public async Task<TipoTarefaViewModel> ConsultarAsync(Guid id)
        {
            BaseRequest request = new BaseRequest
            {
                Id = id.ToString()
            };

            return _mapper.Map<TipoTarefaViewModel>(await _client.ConsultarAsync(request));
        }

        public async Task<IEnumerable<TipoTarefaViewModel>> ListarAsync()
        {
            ListarReply response = await _client.ListarAsync(new Empty());
            return _mapper.Map<IEnumerable<TipoTarefaViewModel>>(response.Lista);
        }

        public async Task<bool> AlterarAsync(TipoTarefaViewModel obj)
        {
            BaseReply reply = await _client.AlterarAsync(_mapper.Map<TipoTarefaModel>(obj));

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
    }
}
