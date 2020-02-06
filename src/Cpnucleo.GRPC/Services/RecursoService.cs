using AutoMapper;
using Cpnucleo.Application.Interfaces;
using Cpnucleo.Infra.CrossCutting.Communication.GRPC.Protos;
using Cpnucleo.Infra.CrossCutting.Util.ViewModels;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cpnucleo.GRPC
{
    public class RecursoService : Recurso.RecursoBase
    {
        private readonly IMapper _mapper;
        private readonly IRecursoAppService _recursoAppService;

        public RecursoService(IMapper mapper, IRecursoAppService recursoAppService)
        {
            _mapper = mapper;
            _recursoAppService = recursoAppService;
        }

        public override async Task<BaseReply> Incluir(RecursoModel request, ServerCallContext context)
        {
            return await Task.FromResult(new BaseReply
            {
                Sucesso = _recursoAppService.Incluir(_mapper.Map<RecursoViewModel>(request))
            });
        }

        public override async Task Listar(Empty request, IServerStreamWriter<RecursoModel> responseStream, ServerCallContext context)
        {
            foreach (RecursoModel item in _mapper.Map<IEnumerable<RecursoModel>>(_recursoAppService.Listar()))
            {
                await responseStream.WriteAsync(item);
            }
        }

        public override async Task<RecursoModel> Consultar(BaseRequest request, ServerCallContext context)
        {
            Guid id = new Guid(request.Id);
            RecursoModel result = _mapper.Map<RecursoModel>(_recursoAppService.Consultar(id));

            return await Task.FromResult(result);
        }

        public override async Task<BaseReply> Alterar(RecursoModel request, ServerCallContext context)
        {
            return await Task.FromResult(new BaseReply
            {
                Sucesso = _recursoAppService.Alterar(_mapper.Map<RecursoViewModel>(request))
            });
        }

        public override async Task<BaseReply> Remover(BaseRequest request, ServerCallContext context)
        {
            return await Task.FromResult(new BaseReply
            {
                Sucesso = _recursoAppService.Remover(new Guid(request.Id))
            });
        }

        public override async Task<RecursoModel> Autenticar(AutenticarRequest request, ServerCallContext context)
        {
            RecursoModel result = _mapper.Map<RecursoModel>(_recursoAppService.Autenticar(request.Login, request.Senha, out _));

            return await Task.FromResult(result);
        }
    }
}
