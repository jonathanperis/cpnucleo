using AutoMapper;
using Cpnucleo.Application.Interfaces;
using Cpnucleo.Infra.CrossCutting.Communication.GRPC.Protos;
using Cpnucleo.Infra.CrossCutting.Communication.GRPC.Protos.Workflow;
using Cpnucleo.Infra.CrossCutting.Util.ViewModels;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cpnucleo.GRPC
{
    public class WorkflowService : Workflow.WorkflowBase
    {
        private readonly IMapper _mapper;
        private readonly IWorkflowAppService _workflowAppService;

        public WorkflowService(IMapper mapper, IWorkflowAppService workflowAppService)
        {
            _mapper = mapper;
            _workflowAppService = workflowAppService;
        }

        public override async Task<BaseReply> Incluir(WorkflowModel request, ServerCallContext context)
        {
            return await Task.FromResult(new BaseReply
            {
                Sucesso = _workflowAppService.Incluir(_mapper.Map<WorkflowViewModel>(request))
            });
        }

        public override async Task<ListarReply> Listar(Empty request, ServerCallContext context)
        {
            ListarReply result = new ListarReply();
            result.Lista.AddRange(_mapper.Map<IEnumerable<WorkflowModel>>(_workflowAppService.Listar()));

            return await Task.FromResult(result);
        }

        public override async Task<WorkflowModel> Consultar(BaseRequest request, ServerCallContext context)
        {
            Guid id = new Guid(request.Id);
            WorkflowModel result = _mapper.Map<WorkflowModel>(_workflowAppService.Consultar(id));

            return await Task.FromResult(result);
        }

        public override async Task<BaseReply> Alterar(WorkflowModel request, ServerCallContext context)
        {
            return await Task.FromResult(new BaseReply
            {
                Sucesso = _workflowAppService.Alterar(_mapper.Map<WorkflowViewModel>(request))
            });
        }

        public override async Task<BaseReply> Remover(BaseRequest request, ServerCallContext context)
        {
            return await Task.FromResult(new BaseReply
            {
                Sucesso = _workflowAppService.Remover(new Guid(request.Id))
            });
        }
    }
}
