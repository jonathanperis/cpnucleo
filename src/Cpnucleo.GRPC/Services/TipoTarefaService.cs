using AutoMapper;
using Cpnucleo.Domain.UoW;
using Cpnucleo.GRPC.Protos;
using Cpnucleo.GRPC.Protos.TipoTarefaProto;
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
    public class TipoTarefaService : TipoTarefaProto.TipoTarefaProtoBase
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public TipoTarefaService(IMapper mapper, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public override async Task<TipoTarefaModel> Incluir(TipoTarefaModel request, ServerCallContext context)
        {
            TipoTarefaModel result = _mapper.Map<TipoTarefaModel>(_mapper.Map<Apontamento>(request));

            return await Task.FromResult(result);
        }

        public override async Task<ListarReply> Listar(Empty request, ServerCallContext context)
        {
            ListarReply result = new ListarReply();
            result.Lista.AddRange(_mapper.Map<IEnumerable<TipoTarefaModel>>(_unitOfWork.TipoTarefaRepository.All()));

            return await Task.FromResult(result);
        }

        public override async Task<TipoTarefaModel> Consultar(BaseRequest request, ServerCallContext context)
        {
            Guid id = new Guid(request.Id);
            TipoTarefaModel result = _mapper.Map<TipoTarefaModel>(_unitOfWork.TipoTarefaRepository.Get(id));

            return await Task.FromResult(result);
        }

        public override async Task<BaseReply> Alterar(TipoTarefaModel request, ServerCallContext context)
        {
            return await Task.FromResult(new BaseReply
            {
                Sucesso = _unitOfWork.TipoTarefaRepository.Update(_mapper.Map<TipoTarefa>(request))
            });
        }

        public override async Task<BaseReply> Remover(BaseRequest request, ServerCallContext context)
        {
            return await Task.FromResult(new BaseReply
            {
                Sucesso = _unitOfWork.TipoTarefaRepository.Remove(new Guid(request.Id))
            });
        }
    }
}
