using AutoMapper;
using Cpnucleo.GRPC;
using Cpnucleo.Infra.CrossCutting.Communication.GRPC.Interfaces;
using Cpnucleo.Infra.CrossCutting.Util.ViewModels;
using Grpc.Core;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cpnucleo.Infra.CrossCutting.Communication.GRPC.Services
{
    public class SistemaGrpcService : BaseGrpcService, ISistemaGrpcService
    {
        private readonly Sistema.SistemaClient _client;

        public SistemaGrpcService(IMapper mapper)
            : base(mapper)
        {
            _client = new Sistema.SistemaClient(_channel);
        }

        public async Task<bool> IncluirAsync(SistemaViewModel obj)
        {
            IncluirReply result = await _client.IncluirAsync(_mapper.Map<SistemaModel>(obj));

            return result.Sucesso;
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

            using (AsyncServerStreamingCall<SistemaModel> reply = _client.Listar(new ListarRequest()))
            {
                while (await reply.ResponseStream.MoveNext())
                {
                    result.Add(_mapper.Map<SistemaViewModel>(reply.ResponseStream.Current));
                }
            }

            return result;
        }
    }
}
