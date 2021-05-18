using AutoMapper;
using Cpnucleo.Domain.UoW;
using Cpnucleo.GRPC.Protos;
using Cpnucleo.GRPC.Protos.WorkflowProto;
using Cpnucleo.Domain.Entities;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace Cpnucleo.GRPC
{
    [Authorize]    
    public class WorkflowService : WorkflowProto.WorkflowProtoBase
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public WorkflowService(IMapper mapper, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public override async Task<WorkflowModel> Incluir(WorkflowModel request, ServerCallContext context)
        {
            WorkflowModel result = _mapper.Map<WorkflowModel>(_mapper.Map<Apontamento>(request));

            return await Task.FromResult(result);
        }

        public override async Task<ListarReply> Listar(Empty request, ServerCallContext context)
        {
            ListarReply result = new ListarReply();
            result.Lista.AddRange(_mapper.Map<IEnumerable<WorkflowModel>>(_unitOfWork.WorkflowRepository.All()));

            int colunas = _unitOfWork.WorkflowRepository.GetQuantidadeColunas();

            foreach (WorkflowModel item in result.Lista)
            {
                item.TamanhoColuna = _unitOfWork.WorkflowRepository.GetTamanhoColuna(colunas);
            }

            return await Task.FromResult(result);
        }

        public override async Task<WorkflowModel> Consultar(BaseRequest request, ServerCallContext context)
        {
            Guid id = new Guid(request.Id);
            WorkflowModel result = _mapper.Map<WorkflowModel>(_unitOfWork.WorkflowRepository.Get(id));

            if (result == null)
            {

            }

            int colunas = _unitOfWork.WorkflowRepository.GetQuantidadeColunas();

            result.TamanhoColuna = _unitOfWork.WorkflowRepository.GetTamanhoColuna(colunas); 

            return await Task.FromResult(result);
        }

        public override async Task<BaseReply> Alterar(WorkflowModel request, ServerCallContext context)
        {
            return await Task.FromResult(new BaseReply
            {
                Sucesso = _unitOfWork.WorkflowRepository.Update(_mapper.Map<Workflow>(request))
            });
        }

        public override async Task<BaseReply> Remover(BaseRequest request, ServerCallContext context)
        {
            return await Task.FromResult(new BaseReply
            {
                Sucesso = _unitOfWork.WorkflowRepository.Remove(new Guid(request.Id))
            });
        }
    }
}
