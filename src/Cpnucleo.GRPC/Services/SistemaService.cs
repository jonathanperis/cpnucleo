using AutoMapper;
using Cpnucleo.Domain.UoW;
using Cpnucleo.GRPC.Protos;
using Cpnucleo.GRPC.Protos.SistemaProto;
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
    public class SistemaService : SistemaProto.SistemaProtoBase
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public SistemaService(IMapper mapper, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public override async Task<SistemaModel> Incluir(SistemaModel request, ServerCallContext context)
        {
            SistemaModel result = _mapper.Map<SistemaModel>(_mapper.Map<Apontamento>(request));

            return await Task.FromResult(result);
        }

        public override async Task<ListarReply> Listar(Empty request, ServerCallContext context)
        {
            ListarReply result = new ListarReply();
            result.Lista.AddRange(_mapper.Map<IEnumerable<SistemaModel>>(_unitOfWork.SistemaRepository.All()));

            return await Task.FromResult(result);
        }

        public override async Task<SistemaModel> Consultar(BaseRequest request, ServerCallContext context)
        {
            Guid id = new Guid(request.Id);
            SistemaModel result = _mapper.Map<SistemaModel>(_unitOfWork.SistemaRepository.Get(id));

            return await Task.FromResult(result);
        }

        public override async Task<BaseReply> Alterar(SistemaModel request, ServerCallContext context)
        {
            return await Task.FromResult(new BaseReply
            {
                Sucesso = _unitOfWork.SistemaRepository.Update(_mapper.Map<Sistema>(request))
            });
        }

        public override async Task<BaseReply> Remover(BaseRequest request, ServerCallContext context)
        {
            return await Task.FromResult(new BaseReply
            {
                Sucesso = _unitOfWork.SistemaRepository.Remove(new Guid(request.Id))
            });
        }
    }
}
