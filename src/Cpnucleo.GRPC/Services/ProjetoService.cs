using AutoMapper;
using Cpnucleo.Domain.UoW;
using Cpnucleo.GRPC.Protos;
using Cpnucleo.GRPC.Protos.ProjetoProto;
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
    public class ProjetoService : ProjetoProto.ProjetoProtoBase
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public ProjetoService(IMapper mapper, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public override async Task<ProjetoModel> Incluir(ProjetoModel request, ServerCallContext context)
        {
            ProjetoModel result = _mapper.Map<ProjetoModel>(_mapper.Map<Apontamento>(request));

            return await Task.FromResult(result);
        }

        public override async Task<ListarReply> Listar(Empty request, ServerCallContext context)
        {
            ListarReply result = new ListarReply();
            result.Lista.AddRange(_mapper.Map<IEnumerable<ProjetoModel>>(_unitOfWork.ProjetoRepository.All(true)));

            return await Task.FromResult(result);
        }

        public override async Task<ProjetoModel> Consultar(BaseRequest request, ServerCallContext context)
        {
            Guid id = new Guid(request.Id);
            ProjetoModel result = _mapper.Map<ProjetoModel>(_unitOfWork.ProjetoRepository.Get(id));

            return await Task.FromResult(result);
        }

        public override async Task<BaseReply> Alterar(ProjetoModel request, ServerCallContext context)
        {
            return await Task.FromResult(new BaseReply
            {
                Sucesso = _unitOfWork.ProjetoRepository.Update(_mapper.Map<Projeto>(request))
            });
        }

        public override async Task<BaseReply> Remover(BaseRequest request, ServerCallContext context)
        {
            return await Task.FromResult(new BaseReply
            {
                Sucesso = _unitOfWork.ProjetoRepository.Remove(new Guid(request.Id))
            });
        }
    }
}
