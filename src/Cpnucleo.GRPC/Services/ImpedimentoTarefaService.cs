using AutoMapper;
using Cpnucleo.Domain.UoW;
using Cpnucleo.GRPC.Protos;
using Cpnucleo.GRPC.Protos.ImpedimentoTarefaProto;
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
    public class ImpedimentoTarefaService : ImpedimentoTarefaProto.ImpedimentoTarefaProtoBase
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public ImpedimentoTarefaService(IMapper mapper, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public override async Task<ImpedimentoTarefaModel> Incluir(ImpedimentoTarefaModel request, ServerCallContext context)
        {
            ImpedimentoTarefaModel result = _mapper.Map<ImpedimentoTarefaModel>(_mapper.Map<Apontamento>(request));

            return await Task.FromResult(result);
        }

        public override async Task<ListarReply> Listar(Empty request, ServerCallContext context)
        {
            ListarReply result = new ListarReply();
            result.Lista.AddRange(_mapper.Map<IEnumerable<ImpedimentoTarefaModel>>(_unitOfWork.ImpedimentoTarefaRepository.AllAsync()));

            return await Task.FromResult(result);
        }

        public override async Task<ImpedimentoTarefaModel> Consultar(BaseRequest request, ServerCallContext context)
        {
            Guid id = new Guid(request.Id);
            ImpedimentoTarefaModel result = _mapper.Map<ImpedimentoTarefaModel>(_unitOfWork.ImpedimentoTarefaRepository.GetAsync(id));

            return await Task.FromResult(result);
        }

        public override async Task<BaseReply> Alterar(ImpedimentoTarefaModel request, ServerCallContext context)
        {
            return await Task.FromResult(new BaseReply
            {
                Sucesso = _unitOfWork.ImpedimentoTarefaRepository.Update(_mapper.Map<ImpedimentoTarefa>(request))
            });
        }

        public override async Task<BaseReply> Remover(BaseRequest request, ServerCallContext context)
        {
            return await Task.FromResult(new BaseReply
            {
                Sucesso = _unitOfWork.ImpedimentoTarefaRepository.Remove(new Guid(request.Id))
            });
        }

        public override async Task<ListarPorTarefaReply> ListarPorTarefa(BaseRequest request, ServerCallContext context)
        {
            Guid id = new Guid(request.Id);
            ListarPorTarefaReply result = new ListarPorTarefaReply();
            result.Lista.AddRange(_mapper.Map<IEnumerable<ImpedimentoTarefaModel>>(_unitOfWork.ImpedimentoTarefaRepository.GetByTarefaAsync(id)));

            return await Task.FromResult(result);
        }
    }
}
