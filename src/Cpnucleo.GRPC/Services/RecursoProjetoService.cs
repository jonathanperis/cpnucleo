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
    public class RecursoProjetoService : RecursoProjeto.RecursoProjetoBase
    {
        private readonly IMapper _mapper;
        private readonly IRecursoProjetoAppService _recursoProjetoAppService;

        public RecursoProjetoService(IMapper mapper, IRecursoProjetoAppService recursoProjetoAppService)
        {
            _mapper = mapper;
            _recursoProjetoAppService = recursoProjetoAppService;
        }

        public override async Task<BaseReply> Incluir(RecursoProjetoModel request, ServerCallContext context)
        {
            return await Task.FromResult(new BaseReply
            {
                Sucesso = _recursoProjetoAppService.Incluir(_mapper.Map<RecursoProjetoViewModel>(request))
            });
        }

        public override async Task Listar(Empty request, IServerStreamWriter<RecursoProjetoModel> responseStream, ServerCallContext context)
        {
            foreach (RecursoProjetoModel item in _mapper.Map<IEnumerable<RecursoProjetoModel>>(_recursoProjetoAppService.Listar()))
            {
                await responseStream.WriteAsync(item);
            }
        }

        public override async Task<RecursoProjetoModel> Consultar(BaseRequest request, ServerCallContext context)
        {
            Guid id = new Guid(request.Id);
            RecursoProjetoModel result = _mapper.Map<RecursoProjetoModel>(_recursoProjetoAppService.Consultar(id));

            return await Task.FromResult(result);
        }

        public override async Task<BaseReply> Alterar(RecursoProjetoModel request, ServerCallContext context)
        {
            return await Task.FromResult(new BaseReply
            {
                Sucesso = _recursoProjetoAppService.Alterar(_mapper.Map<RecursoProjetoViewModel>(request))
            });
        }

        public override async Task<BaseReply> Remover(BaseRequest request, ServerCallContext context)
        {
            return await Task.FromResult(new BaseReply
            {
                Sucesso = _recursoProjetoAppService.Remover(new Guid(request.Id))
            });
        }

        public override async Task ListarPorProjeto(BaseRequest request, IServerStreamWriter<RecursoProjetoModel> responseStream, ServerCallContext context)
        {
            foreach (RecursoProjetoModel item in _mapper.Map<IEnumerable<RecursoProjetoModel>>(_recursoProjetoAppService.ListarPorProjeto(new Guid(request.Id))))
            {
                await responseStream.WriteAsync(item);
            }
        }
    }
}
