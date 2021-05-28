using AutoMapper;
using Cpnucleo.Domain.UoW;
using Cpnucleo.GRPC.Protos;
using Cpnucleo.GRPC.Protos.RecursoTarefaProto;
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
    public class RecursoTarefaService : RecursoTarefaProto.RecursoTarefaProtoBase
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public RecursoTarefaService(IMapper mapper, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public override async Task<RecursoTarefaModel> Incluir(RecursoTarefaModel request, ServerCallContext context)
        {
            RecursoTarefaModel result = _mapper.Map<RecursoTarefaModel>(_mapper.Map<Apontamento>(request));

            return await Task.FromResult(result);
        }

        public override async Task<ListarReply> Listar(Empty request, ServerCallContext context)
        {
            ListarReply result = new ListarReply();
            result.Lista.AddRange(_mapper.Map<IEnumerable<RecursoTarefaModel>>(_unitOfWork.RecursoTarefaRepository.AllAsync()));

            return await Task.FromResult(result);
        }

        public override async Task<RecursoTarefaModel> Consultar(BaseRequest request, ServerCallContext context)
        {
            Guid id = new Guid(request.Id);
            RecursoTarefaModel result = _mapper.Map<RecursoTarefaModel>(_unitOfWork.RecursoTarefaRepository.GetAsync(id));

            return await Task.FromResult(result);
        }

        public override async Task<BaseReply> Alterar(RecursoTarefaModel request, ServerCallContext context)
        {
            return await Task.FromResult(new BaseReply
            {
                Sucesso = _unitOfWork.RecursoTarefaRepository.Update(_mapper.Map<RecursoTarefa>(request))
            });
        }

        public override async Task<BaseReply> Remover(BaseRequest request, ServerCallContext context)
        {
            return await Task.FromResult(new BaseReply
            {
                Sucesso = _unitOfWork.RecursoTarefaRepository.Remove(new Guid(request.Id))
            });
        }

        public override async Task<ListarPorTarefaReply> ListarPorTarefa(BaseRequest request, ServerCallContext context)
        {
            Guid id = new Guid(request.Id);
            ListarPorTarefaReply result = new ListarPorTarefaReply();
            result.Lista.AddRange(_mapper.Map<IEnumerable<RecursoTarefaModel>>(_unitOfWork.RecursoTarefaRepository.GetByTarefaAsync(id)));

            return await Task.FromResult(result);
        }
    }
}
