using AutoMapper;
using Cpnucleo.Application.Interfaces;
using Cpnucleo.Infra.CrossCutting.Communication.GRPC.Protos;
using Cpnucleo.Infra.CrossCutting.Communication.GRPC.Protos.TipoTarefa;
using Cpnucleo.Infra.CrossCutting.Util.ViewModels;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cpnucleo.GRPC
{
    public class TipoTarefaService : TipoTarefa.TipoTarefaBase
    {
        private readonly IMapper _mapper;
        private readonly ICrudAppService<TipoTarefaViewModel> _tipoTarefaAppService;

        public TipoTarefaService(IMapper mapper, ICrudAppService<TipoTarefaViewModel> tipoTarefaAppService)
        {
            _mapper = mapper;
            _tipoTarefaAppService = tipoTarefaAppService;
        }

        public override async Task<BaseReply> Incluir(TipoTarefaModel request, ServerCallContext context)
        {
            return await Task.FromResult(new BaseReply
            {
                Sucesso = _tipoTarefaAppService.Incluir(_mapper.Map<TipoTarefaViewModel>(request))
            });
        }

        public override async Task<ListarReply> Listar(Empty request, ServerCallContext context)
        {
            ListarReply result = new ListarReply();
            result.Lista.AddRange(_mapper.Map<IEnumerable<TipoTarefaModel>>(_tipoTarefaAppService.Listar()));

            return await Task.FromResult(result);
        }

        public override async Task<TipoTarefaModel> Consultar(BaseRequest request, ServerCallContext context)
        {
            Guid id = new Guid(request.Id);
            TipoTarefaModel result = _mapper.Map<TipoTarefaModel>(_tipoTarefaAppService.Consultar(id));

            return await Task.FromResult(result);
        }

        public override async Task<BaseReply> Alterar(TipoTarefaModel request, ServerCallContext context)
        {
            return await Task.FromResult(new BaseReply
            {
                Sucesso = _tipoTarefaAppService.Alterar(_mapper.Map<TipoTarefaViewModel>(request))
            });
        }

        public override async Task<BaseReply> Remover(BaseRequest request, ServerCallContext context)
        {
            return await Task.FromResult(new BaseReply
            {
                Sucesso = _tipoTarefaAppService.Remover(new Guid(request.Id))
            });
        }
    }
}
