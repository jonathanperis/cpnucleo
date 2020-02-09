using AutoMapper;
using Cpnucleo.Infra.CrossCutting.Communication.GRPC.Interfaces;
using Cpnucleo.Infra.CrossCutting.Communication.GRPC.Protos;
using Cpnucleo.Infra.CrossCutting.Communication.GRPC.Protos.RecursoProjeto;
using Cpnucleo.Infra.CrossCutting.Util.ViewModels;
using Google.Protobuf.WellKnownTypes;
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
            ListarReply response = await _client.ListarAsync(new Empty());
            return _mapper.Map<IEnumerable<RecursoProjetoViewModel>>(response.Lista);
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

            ListarPorProjetoReply response = await _client.ListarPorProjetoAsync(request);
            return _mapper.Map<IEnumerable<RecursoProjetoViewModel>>(response.Lista);
        }
    }
}
