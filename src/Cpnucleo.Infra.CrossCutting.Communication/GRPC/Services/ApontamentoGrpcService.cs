using AutoMapper;
using Cpnucleo.Infra.CrossCutting.Communication.GRPC.Interfaces;
using Cpnucleo.Infra.CrossCutting.Communication.GRPC.Protos;
using Cpnucleo.Infra.CrossCutting.Communication.GRPC.Protos.Apontamento;
using Cpnucleo.Infra.CrossCutting.Util.Interfaces;
using Cpnucleo.Infra.CrossCutting.Util.ViewModels;
using Google.Protobuf.WellKnownTypes;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cpnucleo.Infra.CrossCutting.Communication.GRPC.Services
{
    public class ApontamentoGrpcService : BaseGrpcService, IApontamentoGrpcService
    {
        private readonly Apontamento.ApontamentoClient _client;

        public ApontamentoGrpcService(IMapper mapper, ISystemConfiguration systemConfiguration)
            : base(mapper, systemConfiguration)
        {
            _client = new Apontamento.ApontamentoClient(_channel);
        }

        public async Task<bool> IncluirAsync(ApontamentoViewModel obj)
        {
            BaseReply reply = await _client.IncluirAsync(_mapper.Map<ApontamentoModel>(obj));

            return reply.Sucesso;
        }

        public async Task<ApontamentoViewModel> ConsultarAsync(Guid id)
        {
            BaseRequest request = new BaseRequest
            {
                Id = id.ToString()
            };

            return _mapper.Map<ApontamentoViewModel>(await _client.ConsultarAsync(request));
        }

        public async Task<IEnumerable<ApontamentoViewModel>> ListarAsync()
        {
            ListarReply response = await _client.ListarAsync(new Empty());
            return _mapper.Map<IEnumerable<ApontamentoViewModel>>(response.Lista);
        }

        public async Task<bool> AlterarAsync(ApontamentoViewModel obj)
        {
            BaseReply reply = await _client.AlterarAsync(_mapper.Map<ApontamentoModel>(obj));

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

        public async Task<IEnumerable<ApontamentoViewModel>> ListarPorRecursoAsync(Guid id)
        {
            BaseRequest request = new BaseRequest
            {
                Id = id.ToString()
            };

            ListarPorRecursoReply response = await _client.ListarPorRecursoAsync(request);
            return _mapper.Map<IEnumerable<ApontamentoViewModel>>(response.Lista);
        }
    }
}
