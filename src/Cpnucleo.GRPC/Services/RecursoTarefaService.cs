using AutoMapper;
using Cpnucleo.Application.Interfaces;
using Cpnucleo.Infra.CrossCutting.Communication.GRPC.Protos;
using Cpnucleo.Infra.CrossCutting.Util.ViewModels;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cpnucleo.GRPC
{
    public class RecursoTarefaService : RecursoTarefa.RecursoTarefaBase
    {
        private readonly IMapper _mapper;
        private readonly IRecursoTarefaAppService _recursoTarefaAppService;

        public RecursoTarefaService(IMapper mapper, IRecursoTarefaAppService recursoTarefaAppService)
        {
            _mapper = mapper;
            _recursoTarefaAppService = recursoTarefaAppService;
        }

        public override async Task<BaseReply> Incluir(RecursoTarefaModel request, ServerCallContext context)
        {
            return await Task.FromResult(new BaseReply
            {
                Sucesso = _recursoTarefaAppService.Incluir(_mapper.Map<RecursoTarefaViewModel>(request))
            });
        }

        public override async Task Listar(Empty request, IServerStreamWriter<RecursoTarefaModel> responseStream, ServerCallContext context)
        {
            foreach (RecursoTarefaModel item in _mapper.Map<IEnumerable<RecursoTarefaModel>>(_recursoTarefaAppService.Listar()))
            {
                await responseStream.WriteAsync(item);
            }
        }

        public override async Task<RecursoTarefaModel> Consultar(BaseRequest request, ServerCallContext context)
        {
            Guid id = new Guid(request.Id);
            RecursoTarefaModel result = _mapper.Map<RecursoTarefaModel>(_recursoTarefaAppService.Consultar(id));

            return await Task.FromResult(result);
        }

        public override async Task<BaseReply> Alterar(RecursoTarefaModel request, ServerCallContext context)
        {
            return await Task.FromResult(new BaseReply
            {
                Sucesso = _recursoTarefaAppService.Alterar(_mapper.Map<RecursoTarefaViewModel>(request))
            });
        }

        public override async Task<BaseReply> Remover(BaseRequest request, ServerCallContext context)
        {
            return await Task.FromResult(new BaseReply
            {
                Sucesso = _recursoTarefaAppService.Remover(new Guid(request.Id))
            });
        }

        public override async Task ListarPorTarefa(BaseRequest request, IServerStreamWriter<RecursoTarefaModel> responseStream, ServerCallContext context)
        {
            foreach (RecursoTarefaModel item in _mapper.Map<IEnumerable<RecursoTarefaModel>>(_recursoTarefaAppService.ListarPorTarefa(new Guid(request.Id))))
            {
                await responseStream.WriteAsync(item);
            }
        }

        public override async Task ListarPorRecurso(BaseRequest request, IServerStreamWriter<RecursoTarefaModel> responseStream, ServerCallContext context)
        {
            foreach (RecursoTarefaModel item in _mapper.Map<IEnumerable<RecursoTarefaModel>>(_recursoTarefaAppService.ListarPorRecurso(new Guid(request.Id))))
            {
                await responseStream.WriteAsync(item);
            }
        }
    }
}
