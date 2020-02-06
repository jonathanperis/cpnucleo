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
    public class SistemaGrpcService : BaseGrpcService, ICrudGrpcService<SistemaViewModel>
    {
        private readonly Sistema.SistemaClient _client;

        public SistemaGrpcService(IMapper mapper)
            : base(mapper)
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
            List<SistemaViewModel> result = new List<SistemaViewModel>();

            //using (var reply = _client.Listar(new ListarRequest()))
            //{
            //    await foreach (var item in reply.ResponseStream.ReadAllAsync())
            //    {
            //        result.Add(_mapper.Map<SistemaViewModel>(item));
            //    }
            //}

            using (AsyncServerStreamingCall<SistemaModel> reply = _client.Listar(new Empty()))
            {
                while (await reply.ResponseStream.MoveNext())
                {
                    result.Add(_mapper.Map<SistemaViewModel>(reply.ResponseStream.Current));
                }
            }

            return result;
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
