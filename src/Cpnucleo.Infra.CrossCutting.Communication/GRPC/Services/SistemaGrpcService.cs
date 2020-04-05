using AutoMapper;
using Cpnucleo.Infra.CrossCutting.Communication.GRPC.Interfaces;
using Cpnucleo.Infra.CrossCutting.Communication.GRPC.Protos;
using Cpnucleo.Infra.CrossCutting.Communication.GRPC.Protos.Sistema;
using Cpnucleo.Infra.CrossCutting.Util.Interfaces;
using Cpnucleo.Infra.CrossCutting.Util.ViewModels;
using Google.Protobuf.WellKnownTypes;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cpnucleo.Infra.CrossCutting.Communication.GRPC.Services
{
    internal class SistemaGrpcService : BaseGrpcService, ICrudGrpcService<SistemaViewModel>
    {
        private readonly Sistema.SistemaClient _client;

        public SistemaGrpcService(IMapper mapper, ISystemConfiguration systemConfiguration)
            : base(mapper, systemConfiguration)
        {
            _client = new Sistema.SistemaClient(_channel);
        }

        public async Task<bool> IncluirAsync(SistemaViewModel obj)
        {
            BaseReply reply = await _client.IncluirAsync(_mapper.Map<SistemaModel>(obj));

            return reply.Sucesso;
        }

        public async Task<SistemaViewModel> ConsultarAsync(Guid id)
        {
            BaseRequest request = new BaseRequest
            {
                Id = id.ToString()
            };

            return _mapper.Map<SistemaViewModel>(await _client.ConsultarAsync(request));
        }

        public async Task<IEnumerable<SistemaViewModel>> ListarAsync()
        {
            ListarReply response = await _client.ListarAsync(new Empty());
            return _mapper.Map<IEnumerable<SistemaViewModel>>(response.Lista);
        }

        public async Task<bool> AlterarAsync(SistemaViewModel obj)
        {
            BaseReply reply = await _client.AlterarAsync(_mapper.Map<SistemaModel>(obj));

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
