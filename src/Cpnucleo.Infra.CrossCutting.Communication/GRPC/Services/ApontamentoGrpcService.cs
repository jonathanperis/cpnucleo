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
    public class ApontamentoGrpcService : BaseGrpcService, IApontamentoGrpcService
    {
        private readonly Apontamento.ApontamentoClient _client;

        public ApontamentoGrpcService(IMapper mapper)
            : base(mapper)
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
            List<ApontamentoViewModel> result = new List<ApontamentoViewModel>();

            //using (var reply = _client.Listar(new ListarRequest()))
            //{
            //    await foreach (var item in reply.ResponseStream.ReadAllAsync())
            //    {
            //        result.Add(_mapper.Map<ApontamentoViewModel>(item));
            //    }
            //}

            using (AsyncServerStreamingCall<ApontamentoModel> reply = _client.Listar(new Empty()))
            {
                while (await reply.ResponseStream.MoveNext())
                {
                    result.Add(_mapper.Map<ApontamentoViewModel>(reply.ResponseStream.Current));
                }
            }

            return result;
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

        public Task<IEnumerable<ApontamentoViewModel>> ListarPorRecursoAsync(Guid id)
        {
            throw new NotImplementedException();
        }
    }
}
