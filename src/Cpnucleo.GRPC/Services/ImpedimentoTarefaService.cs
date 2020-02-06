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
    public class ImpedimentoTarefaService : ImpedimentoTarefa.ImpedimentoTarefaBase
    {
        private readonly IMapper _mapper;
        private readonly IImpedimentoTarefaAppService _impedimentoTarefaAppService;

        public ImpedimentoTarefaService(IMapper mapper, IImpedimentoTarefaAppService impedimentoTarefaAppService)
        {
            _mapper = mapper;
            _impedimentoTarefaAppService = impedimentoTarefaAppService;
        }

        public override async Task<BaseReply> Incluir(ImpedimentoTarefaModel request, ServerCallContext context)
        {
            return await Task.FromResult(new BaseReply
            {
                Sucesso = _impedimentoTarefaAppService.Incluir(_mapper.Map<ImpedimentoTarefaViewModel>(request))
            });
        }

        public override async Task Listar(Empty request, IServerStreamWriter<ImpedimentoTarefaModel> responseStream, ServerCallContext context)
        {
            foreach (ImpedimentoTarefaModel item in _mapper.Map<IEnumerable<ImpedimentoTarefaModel>>(_impedimentoTarefaAppService.Listar()))
            {
                await responseStream.WriteAsync(item);
            }
        }

        public override async Task<ImpedimentoTarefaModel> Consultar(BaseRequest request, ServerCallContext context)
        {
            Guid id = new Guid(request.Id);
            ImpedimentoTarefaModel result = _mapper.Map<ImpedimentoTarefaModel>(_impedimentoTarefaAppService.Consultar(id));

            return await Task.FromResult(result);
        }

        public override async Task<BaseReply> Alterar(ImpedimentoTarefaModel request, ServerCallContext context)
        {
            return await Task.FromResult(new BaseReply
            {
                Sucesso = _impedimentoTarefaAppService.Alterar(_mapper.Map<ImpedimentoTarefaViewModel>(request))
            });
        }

        public override async Task<BaseReply> Remover(BaseRequest request, ServerCallContext context)
        {
            return await Task.FromResult(new BaseReply
            {
                Sucesso = _impedimentoTarefaAppService.Remover(new Guid(request.Id))
            });
        }

        public override async Task ListarPorTarefa(BaseRequest request, IServerStreamWriter<ImpedimentoTarefaModel> responseStream, ServerCallContext context)
        {
            foreach (ImpedimentoTarefaModel item in _mapper.Map<IEnumerable<ImpedimentoTarefaModel>>(_impedimentoTarefaAppService.ListarPorTarefa(new Guid(request.Id))))
            {
                await responseStream.WriteAsync(item);
            }
        }
    }
}
