using AutoMapper;
using Cpnucleo.Infra.CrossCutting.Communication.GRPC.Interfaces;
using Cpnucleo.Infra.CrossCutting.Communication.GRPC.Protos;
using Cpnucleo.Infra.CrossCutting.Communication.GRPC.Protos.Recurso;
using Cpnucleo.Infra.CrossCutting.Util.Interfaces;
using Cpnucleo.Infra.CrossCutting.Util.ViewModels;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cpnucleo.Infra.CrossCutting.Communication.GRPC.Services
{
    internal class RecursoGrpcService : BaseGrpcService, IRecursoGrpcService
    {
        private readonly Recurso.RecursoClient _client;

        public RecursoGrpcService(IMapper mapper, ISystemConfiguration systemConfiguration)
            : base(mapper, systemConfiguration)
        {
            _client = new Recurso.RecursoClient(_channel);
        }

        public async Task<bool> IncluirAsync(RecursoViewModel obj)
        {
            BaseReply reply = await _client.IncluirAsync(_mapper.Map<RecursoModel>(obj));

            return reply.Sucesso;
        }

        public async Task<RecursoViewModel> ConsultarAsync(Guid id)
        {
            BaseRequest request = new BaseRequest
            {
                Id = id.ToString()
            };

            return _mapper.Map<RecursoViewModel>(await _client.ConsultarAsync(request));
        }

        public async Task<IEnumerable<RecursoViewModel>> ListarAsync()
        {
            List<RecursoViewModel> result = new List<RecursoViewModel>();

            //using (AsyncServerStreamingCall<RecursoModel> reply = _client.Listar(new Empty()))
            //{
            //    await foreach (RecursoModel item in reply.ResponseStream.ReadAllAsync())
            //    {
            //        result.Add(_mapper.Map<RecursoViewModel>(item));
            //    }
            //}

            using (AsyncServerStreamingCall<RecursoModel> reply = _client.Listar(new Empty()))
            {
                while (await reply.ResponseStream.MoveNext())
                {
                    result.Add(_mapper.Map<RecursoViewModel>(reply.ResponseStream.Current));
                }
            }

            return result;
        }

        public async Task<bool> AlterarAsync(RecursoViewModel obj)
        {
            BaseReply reply = await _client.AlterarAsync(_mapper.Map<RecursoModel>(obj));

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

        public async Task<RecursoViewModel> AutenticarAsync(string login, string senha)
        {
            AutenticarRequest request = new AutenticarRequest
            {
                Login = login,
                Senha = senha
            };

            return _mapper.Map<RecursoViewModel>(await _client.AutenticarAsync(request));
        }
    }
}
