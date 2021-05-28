using AutoMapper;
using Cpnucleo.Domain.UoW;
using Cpnucleo.GRPC.Protos;
using Cpnucleo.GRPC.Protos.ImpedimentoProto;
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
    public class ImpedimentoService : ImpedimentoProto.ImpedimentoProtoBase
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public ImpedimentoService(IMapper mapper, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public override async Task<ImpedimentoModel> Incluir(ImpedimentoModel request, ServerCallContext context)
        {
            ImpedimentoModel result = _mapper.Map<ImpedimentoModel>(_mapper.Map<Apontamento>(request));

            return await Task.FromResult(result);
        }

        public override async Task<ListarReply> Listar(Empty request, ServerCallContext context)
        {
            ListarReply result = new ListarReply();
            result.Lista.AddRange(_mapper.Map<IEnumerable<ImpedimentoModel>>(_unitOfWork.ImpedimentoRepository.AllAsync()));

            return await Task.FromResult(result);
        }

        public override async Task<ImpedimentoModel> Consultar(BaseRequest request, ServerCallContext context)
        {
            Guid id = new Guid(request.Id);
            ImpedimentoModel result = _mapper.Map<ImpedimentoModel>(_unitOfWork.ImpedimentoRepository.GetAsync(id));

            return await Task.FromResult(result);
        }

        public override async Task<BaseReply> Alterar(ImpedimentoModel request, ServerCallContext context)
        {
            return await Task.FromResult(new BaseReply
            {
                Sucesso = _unitOfWork.ImpedimentoRepository.Update(_mapper.Map<Impedimento>(request))
            });
        }

        public override async Task<BaseReply> Remover(BaseRequest request, ServerCallContext context)
        {
            return await Task.FromResult(new BaseReply
            {
                Sucesso = _unitOfWork.ImpedimentoRepository.Remove(new Guid(request.Id))
            });
        }
    }
}
