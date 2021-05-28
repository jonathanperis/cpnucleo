using AutoMapper;
using Cpnucleo.Domain.UoW;
using Cpnucleo.GRPC.Protos;
using Cpnucleo.GRPC.Protos.ApontamentoProto;
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
    public class ApontamentoService : ApontamentoProto.ApontamentoProtoBase
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public ApontamentoService(IMapper mapper, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public override async Task<ApontamentoModel> Incluir(ApontamentoModel request, ServerCallContext context)
        {
            ApontamentoModel result = _mapper.Map<ApontamentoModel>(_mapper.Map<Apontamento>(request));

            return await Task.FromResult(result);
        }

        public override async Task<ListarReply> Listar(Empty request, ServerCallContext context)
        {
            ListarReply result = new ListarReply();
            result.Lista.AddRange(_mapper.Map<IEnumerable<ApontamentoModel>>(_unitOfWork.ApontamentoRepository.AllAsync()));

            return await Task.FromResult(result);
        }

        public override async Task<ApontamentoModel> Consultar(BaseRequest request, ServerCallContext context)
        {
            Guid id = new Guid(request.Id);
            ApontamentoModel result = _mapper.Map<ApontamentoModel>(_unitOfWork.ApontamentoRepository.GetAsync(id));

            return await Task.FromResult(result);
        }

        public override async Task<BaseReply> Alterar(ApontamentoModel request, ServerCallContext context)
        {
            return await Task.FromResult(new BaseReply
            {
                Sucesso = _unitOfWork.ApontamentoRepository.Update(_mapper.Map<Apontamento>(request))
            });
        }

        public override async Task<BaseReply> Remover(BaseRequest request, ServerCallContext context)
        {
            return await Task.FromResult(new BaseReply
            {
                Sucesso = _unitOfWork.ApontamentoRepository.RemoveAsync(new Guid(request.Id))
            });
        }

        public override async Task<ListarPorRecursoReply> ListarPorRecurso(BaseRequest request, ServerCallContext context)
        {
            ListarPorRecursoReply result = new ListarPorRecursoReply();
            result.Lista.AddRange(_mapper.Map<IEnumerable<ApontamentoModel>>(_unitOfWork.ApontamentoRepository.GetByRecursoAsync(new Guid(request.Id))));

            return await Task.FromResult(result);
        }
    }
}
