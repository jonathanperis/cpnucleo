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
    public class ImpedimentoGrpcService : BaseGrpcService, ICrudGrpcService<ImpedimentoViewModel>
    {
        private readonly Impedimento.ImpedimentoClient _client;

        public ImpedimentoGrpcService(IMapper mapper)
            : base(mapper)
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
            List<ImpedimentoViewModel> result = new List<ImpedimentoViewModel>();

            using (AsyncServerStreamingCall<ImpedimentoModel> reply = _client.Listar(new Empty()))
            {
                while (await reply.ResponseStream.MoveNext())
                {
                    result.Add(_mapper.Map<ImpedimentoViewModel>(reply.ResponseStream.Current));
                }
            }

            return result;
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
