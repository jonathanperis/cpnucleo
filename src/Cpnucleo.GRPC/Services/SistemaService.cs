using AutoMapper;
using Cpnucleo.Application.Interfaces;
using Cpnucleo.Infra.CrossCutting.Communication.GRPC.Protos;
using Cpnucleo.Infra.CrossCutting.Communication.GRPC.Protos.Sistema;
using Cpnucleo.Infra.CrossCutting.Util.ViewModels;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cpnucleo.GRPC
{
    public class SistemaService : Sistema.SistemaBase
    {
        private readonly IMapper _mapper;
        private readonly ICrudAppService<SistemaViewModel> _sistemaAppService;

        public SistemaService(IMapper mapper, ICrudAppService<SistemaViewModel> sistemaAppService)
        {
            _mapper = mapper;
            _sistemaAppService = sistemaAppService;
        }

        public override async Task<BaseReply> Incluir(SistemaModel request, ServerCallContext context)
        {
            return await Task.FromResult(new BaseReply
            {
                Id = _sistemaAppService.Incluir(_mapper.Map<SistemaViewModel>(request)).ToString()
            });
        }

        public override async Task<ListarReply> Listar(Empty request, ServerCallContext context)
        {
            ListarReply result = new ListarReply();
            result.Lista.AddRange(_mapper.Map<IEnumerable<SistemaModel>>(_sistemaAppService.Listar()));

            return await Task.FromResult(result);
        }

        public override async Task<SistemaModel> Consultar(BaseRequest request, ServerCallContext context)
        {
            Guid id = new Guid(request.Id);
            SistemaModel result = _mapper.Map<SistemaModel>(_sistemaAppService.Consultar(id));

            return await Task.FromResult(result);
        }

        public override async Task<BaseReply> Alterar(SistemaModel request, ServerCallContext context)
        {
            return await Task.FromResult(new BaseReply
            {
                Sucesso = _sistemaAppService.Alterar(_mapper.Map<SistemaViewModel>(request))
            });
        }

        public override async Task<BaseReply> Remover(BaseRequest request, ServerCallContext context)
        {
            return await Task.FromResult(new BaseReply
            {
                Sucesso = _sistemaAppService.Remover(new Guid(request.Id))
            });
        }
    }
}
