using AutoMapper;
using Cpnucleo.Domain.UoW;
using Cpnucleo.GRPC.Protos;
using Cpnucleo.GRPC.Protos.RecursoProjetoProto;
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
    public class RecursoProjetoService : RecursoProjetoProto.RecursoProjetoProtoBase
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public RecursoProjetoService(IMapper mapper, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public override async Task<RecursoProjetoModel> Incluir(RecursoProjetoModel request, ServerCallContext context)
        {
            RecursoProjetoModel result = _mapper.Map<RecursoProjetoModel>(_mapper.Map<Apontamento>(request));

            return await Task.FromResult(result);
        }

        public override async Task<ListarReply> Listar(Empty request, ServerCallContext context)
        {
            ListarReply result = new ListarReply();
            result.Lista.AddRange(_mapper.Map<IEnumerable<RecursoProjetoModel>>(_unitOfWork.RecursoProjetoRepository.AllAsync()));

            return await Task.FromResult(result);
        }

        public override async Task<RecursoProjetoModel> Consultar(BaseRequest request, ServerCallContext context)
        {
            Guid id = new Guid(request.Id);
            RecursoProjetoModel result = _mapper.Map<RecursoProjetoModel>(_unitOfWork.RecursoProjetoRepository.GetAsync(id));

            return await Task.FromResult(result);
        }

        public override async Task<BaseReply> Alterar(RecursoProjetoModel request, ServerCallContext context)
        {
            return await Task.FromResult(new BaseReply
            {
                Sucesso = _unitOfWork.RecursoProjetoRepository.Update(_mapper.Map<RecursoProjeto>(request))
            });
        }

        public override async Task<BaseReply> Remover(BaseRequest request, ServerCallContext context)
        {
            return await Task.FromResult(new BaseReply
            {
                Sucesso = _unitOfWork.RecursoProjetoRepository.Remove(new Guid(request.Id))
            });
        }

        public override async Task<ListarPorProjetoReply> ListarPorProjeto(BaseRequest request, ServerCallContext context)
        {
            Guid id = new Guid(request.Id);
            ListarPorProjetoReply result = new ListarPorProjetoReply();
            result.Lista.AddRange(_mapper.Map<IEnumerable<RecursoProjetoModel>>(_unitOfWork.RecursoProjetoRepository.GetByProjetoAsync(id)));

            return await Task.FromResult(result);
        }
    }
}
