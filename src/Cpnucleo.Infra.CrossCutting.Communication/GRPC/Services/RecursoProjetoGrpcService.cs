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
    public class RecursoProjetoGrpcService : BaseGrpcService, IRecursoProjetoGrpcService
    {
        private readonly RecursoProjeto.RecursoProjetoClient _client;

        public RecursoProjetoGrpcService(IMapper mapper)
            : base(mapper)
        {
            _client = new RecursoProjeto.RecursoProjetoClient(_channel);
        }

        public async Task<bool> IncluirAsync(RecursoProjetoViewModel obj)
        {
            BaseReply reply = await _client.IncluirAsync(_mapper.Map<RecursoProjetoModel>(obj));

            return reply.Sucesso;
        }

        public async Task<RecursoProjetoViewModel> ConsultarAsync(Guid id)
        {
            BaseRequest request = new BaseRequest
            {
                Id = id.ToString()
            };

            return _mapper.Map<RecursoProjetoViewModel>(await _client.ConsultarAsync(request));
        }

        public async Task<IEnumerable<RecursoProjetoViewModel>> ListarAsync()
        {
            List<RecursoProjetoViewModel> result = new List<RecursoProjetoViewModel>();

            using (AsyncServerStreamingCall<RecursoProjetoModel> reply = _client.Listar(new Empty()))
            {
                while (await reply.ResponseStream.MoveNext())
                {
                    result.Add(_mapper.Map<RecursoProjetoViewModel>(reply.ResponseStream.Current));
                }
            }

            return result;
        }

        public async Task<bool> AlterarAsync(RecursoProjetoViewModel obj)
        {
            BaseReply reply = await _client.AlterarAsync(_mapper.Map<RecursoProjetoModel>(obj));

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

        public async Task<IEnumerable<RecursoProjetoViewModel>> ListarPorProjetoAsync(Guid idProjeto)
        {
            BaseRequest request = new BaseRequest
            {
                Id = idProjeto.ToString()
            };

            List<RecursoProjetoViewModel> result = new List<RecursoProjetoViewModel>();

            using (AsyncServerStreamingCall<RecursoProjetoModel> reply = _client.ListarPorProjeto(request))
            {
                while (await reply.ResponseStream.MoveNext())
                {
                    result.Add(_mapper.Map<RecursoProjetoViewModel>(reply.ResponseStream.Current));
                }
            }

            return result;
        }
    }
}
