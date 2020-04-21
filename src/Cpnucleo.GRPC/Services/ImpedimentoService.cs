using AutoMapper;
using Cpnucleo.Application.Interfaces;
using Cpnucleo.Infra.CrossCutting.Communication.GRPC.Protos;
using Cpnucleo.Infra.CrossCutting.Communication.GRPC.Protos.Impedimento;
using Cpnucleo.Infra.CrossCutting.Util.ViewModels;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cpnucleo.GRPC
{
    public class ImpedimentoService : Impedimento.ImpedimentoBase
    {
        private readonly IMapper _mapper;
        private readonly ICrudAppService<ImpedimentoViewModel> _impedimentoAppService;

        public ImpedimentoService(IMapper mapper, ICrudAppService<ImpedimentoViewModel> impedimentoAppService)
        {
            _mapper = mapper;
            _impedimentoAppService = impedimentoAppService;
        }

        public override async Task<BaseReply> Incluir(ImpedimentoModel request, ServerCallContext context)
        {
            return await Task.FromResult(new BaseReply
            {
                Id = _impedimentoAppService.Incluir(_mapper.Map<ImpedimentoViewModel>(request)).ToString()
            });
        }

        public override async Task<ListarReply> Listar(Empty request, ServerCallContext context)
        {
            ListarReply result = new ListarReply();
            result.Lista.AddRange(_mapper.Map<IEnumerable<ImpedimentoModel>>(_impedimentoAppService.Listar()));

            return await Task.FromResult(result);
        }

        public override async Task<ImpedimentoModel> Consultar(BaseRequest request, ServerCallContext context)
        {
            Guid id = new Guid(request.Id);
            ImpedimentoModel result = _mapper.Map<ImpedimentoModel>(_impedimentoAppService.Consultar(id));

            return await Task.FromResult(result);
        }

        public override async Task<BaseReply> Alterar(ImpedimentoModel request, ServerCallContext context)
        {
            return await Task.FromResult(new BaseReply
            {
                Sucesso = _impedimentoAppService.Alterar(_mapper.Map<ImpedimentoViewModel>(request))
            });
        }

        public override async Task<BaseReply> Remover(BaseRequest request, ServerCallContext context)
        {
            return await Task.FromResult(new BaseReply
            {
                Sucesso = _impedimentoAppService.Remover(new Guid(request.Id))
            });
        }
    }
}
