using AutoMapper;
using Cpnucleo.Infra.CrossCutting.Communication.GRPC.Interfaces;
using Cpnucleo.Infra.CrossCutting.Communication.GRPC.Protos;
using Cpnucleo.Infra.CrossCutting.Communication.GRPC.Protos.Impedimento;
using Cpnucleo.Infra.CrossCutting.Util.Interfaces;
using Cpnucleo.Infra.CrossCutting.Util.ViewModels;
using Google.Protobuf.WellKnownTypes;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cpnucleo.Infra.CrossCutting.Communication.GRPC.Services
{
    public class ImpedimentoGrpcService : BaseGrpcService, ICrudGrpcService<ImpedimentoViewModel>
    {
        private readonly Impedimento.ImpedimentoClient _client;

        public ImpedimentoGrpcService(IMapper mapper, ISystemConfiguration systemConfiguration)
            : base(mapper, systemConfiguration)
        {
            _client = new Impedimento.ImpedimentoClient(_channel);
        }

        public async Task<bool> IncluirAsync(ImpedimentoViewModel obj)
        {
            BaseReply reply = await _client.IncluirAsync(_mapper.Map<ImpedimentoModel>(obj));

            return reply.Sucesso;
        }

        public async Task<ImpedimentoViewModel> ConsultarAsync(Guid id)
        {
            BaseRequest request = new BaseRequest
            {
                Id = id.ToString()
            };

            return _mapper.Map<ImpedimentoViewModel>(await _client.ConsultarAsync(request));
        }

        public async Task<IEnumerable<ImpedimentoViewModel>> ListarAsync()
        {
            ListarReply response = await _client.ListarAsync(new Empty());
            return _mapper.Map<IEnumerable<ImpedimentoViewModel>>(response.Lista);
        }

        public async Task<bool> AlterarAsync(ImpedimentoViewModel obj)
        {
            BaseReply reply = await _client.AlterarAsync(_mapper.Map<ImpedimentoModel>(obj));

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
