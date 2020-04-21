using AutoMapper;
using Cpnucleo.Application.Interfaces;
using Cpnucleo.Infra.CrossCutting.Communication.GRPC.Protos;
using Cpnucleo.Infra.CrossCutting.Communication.GRPC.Protos.Tarefa;
using Cpnucleo.Infra.CrossCutting.Util.ViewModels;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cpnucleo.GRPC
{
    public class TarefaService : Tarefa.TarefaBase
    {
        private readonly IMapper _mapper;
        private readonly ITarefaAppService _tarefaAppService;

        public TarefaService(IMapper mapper, ITarefaAppService tarefaAppService)
        {
            _mapper = mapper;
            _tarefaAppService = tarefaAppService;
        }

        public override async Task<BaseReply> Incluir(TarefaModel request, ServerCallContext context)
        {
            return await Task.FromResult(new BaseReply
            {
                Id = _tarefaAppService.Incluir(_mapper.Map<TarefaViewModel>(request)).ToString()
            });
        }

        public override async Task<ListarReply> Listar(Empty request, ServerCallContext context)
        {
            ListarReply result = new ListarReply();
            result.Lista.AddRange(_mapper.Map<IEnumerable<TarefaModel>>(_tarefaAppService.Listar()));

            return await Task.FromResult(result);
        }

        public override async Task<TarefaModel> Consultar(BaseRequest request, ServerCallContext context)
        {
            Guid id = new Guid(request.Id);
            TarefaModel result = _mapper.Map<TarefaModel>(_tarefaAppService.Consultar(id));

            return await Task.FromResult(result);
        }

        public override async Task<BaseReply> Alterar(TarefaModel request, ServerCallContext context)
        {
            return await Task.FromResult(new BaseReply
            {
                Sucesso = _tarefaAppService.Alterar(_mapper.Map<TarefaViewModel>(request))
            });
        }

        public override async Task<BaseReply> Remover(BaseRequest request, ServerCallContext context)
        {
            return await Task.FromResult(new BaseReply
            {
                Sucesso = _tarefaAppService.Remover(new Guid(request.Id))
            });
        }

        public override async Task<ListarPorRecursoReply> ListarPorRecurso(BaseRequest request, ServerCallContext context)
        {
            ListarPorRecursoReply result = new ListarPorRecursoReply();
            result.Lista.AddRange(_mapper.Map<IEnumerable<TarefaModel>>(_tarefaAppService.ListarPorRecurso(new Guid(request.Id))));

            return await Task.FromResult(result);
        }

        public override async Task<BaseReply> AlterarPorWorkflow(AlterarPorWorkflowRequest request, ServerCallContext context)
        {
            Guid idTarefa = new Guid(request.IdTarefa);
            Guid idWorkflow = new Guid(request.IdWorkflow);

            return await Task.FromResult(new BaseReply
            {
                Sucesso = _tarefaAppService.AlterarPorWorkflow(idTarefa, idWorkflow)
            });
        }
    }
}
