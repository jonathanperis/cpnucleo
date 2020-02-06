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
    public class RecursoTarefaGrpcService : BaseGrpcService, IRecursoTarefaGrpcService
    {
        private readonly RecursoTarefa.RecursoTarefaClient _client;

        public RecursoTarefaGrpcService(IMapper mapper)
            : base(mapper)
        {
            _client = new RecursoTarefa.RecursoTarefaClient(_channel);
        }

        public async Task<bool> IncluirAsync(RecursoTarefaViewModel obj)
        {
            BaseReply reply = await _client.IncluirAsync(_mapper.Map<RecursoTarefaModel>(obj));

            return reply.Sucesso;
        }

        public async Task<RecursoTarefaViewModel> ConsultarAsync(Guid id)
        {
            BaseRequest request = new BaseRequest
            {
                Id = id.ToString()
            };

            return _mapper.Map<RecursoTarefaViewModel>(await _client.ConsultarAsync(request));
        }

        public async Task<IEnumerable<RecursoTarefaViewModel>> ListarAsync()
        {
            List<RecursoTarefaViewModel> result = new List<RecursoTarefaViewModel>();

            using (AsyncServerStreamingCall<RecursoTarefaModel> reply = _client.Listar(new Empty()))
            {
                while (await reply.ResponseStream.MoveNext())
                {
                    result.Add(_mapper.Map<RecursoTarefaViewModel>(reply.ResponseStream.Current));
                }
            }

            return result;
        }

        public async Task<bool> AlterarAsync(RecursoTarefaViewModel obj)
        {
            BaseReply reply = await _client.AlterarAsync(_mapper.Map<RecursoTarefaModel>(obj));

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

        public async Task<IEnumerable<RecursoTarefaViewModel>> ListarPorTarefaAsync(Guid idTarefa)
        {
            BaseRequest request = new BaseRequest
            {
                Id = idTarefa.ToString()
            };

            List<RecursoTarefaViewModel> result = new List<RecursoTarefaViewModel>();

            using (AsyncServerStreamingCall<RecursoTarefaModel> reply = _client.ListarPorTarefa(request))
            {
                while (await reply.ResponseStream.MoveNext())
                {
                    result.Add(_mapper.Map<RecursoTarefaViewModel>(reply.ResponseStream.Current));
                }
            }

            return result;
        }

        public async Task<IEnumerable<RecursoTarefaViewModel>> ListarPorRecursoAsync(Guid idRecurso)
        {
            BaseRequest request = new BaseRequest
            {
                Id = idRecurso.ToString()
            };

            List<RecursoTarefaViewModel> result = new List<RecursoTarefaViewModel>();

            using (AsyncServerStreamingCall<RecursoTarefaModel> reply = _client.ListarPorRecurso(request))
            {
                while (await reply.ResponseStream.MoveNext())
                {
                    result.Add(_mapper.Map<RecursoTarefaViewModel>(reply.ResponseStream.Current));
                }
            }

            return result;
        }
    }
}
