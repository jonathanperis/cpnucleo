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
    public class ProjetoGrpcService : BaseGrpcService, ICrudGrpcService<ProjetoViewModel>
    {
        private readonly Projeto.ProjetoClient _client;

        public ProjetoGrpcService(IMapper mapper)
            : base(mapper)
        {
            _client = new Projeto.ProjetoClient(_channel);
        }

        public async Task<bool> IncluirAsync(ProjetoViewModel obj)
        {
            BaseReply reply = await _client.IncluirAsync(_mapper.Map<ProjetoModel>(obj));

            return reply.Sucesso;
        }

        public async Task<ProjetoViewModel> ConsultarAsync(Guid id)
        {
            BaseRequest request = new BaseRequest
            {
                Id = id.ToString()
            };

            return _mapper.Map<ProjetoViewModel>(await _client.ConsultarAsync(request));
        }

        public async Task<IEnumerable<ProjetoViewModel>> ListarAsync()
        {
            List<ProjetoViewModel> result = new List<ProjetoViewModel>();

            using (AsyncServerStreamingCall<ProjetoModel> reply = _client.Listar(new Empty()))
            {
                while (await reply.ResponseStream.MoveNext())
                {
                    result.Add(_mapper.Map<ProjetoViewModel>(reply.ResponseStream.Current));
                }
            }

            return result;
        }

        public async Task<bool> AlterarAsync(ProjetoViewModel obj)
        {
            BaseReply reply = await _client.AlterarAsync(_mapper.Map<ProjetoModel>(obj));

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
