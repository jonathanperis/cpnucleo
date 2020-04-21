using AutoMapper;
using Cpnucleo.Application.Interfaces;
using Cpnucleo.Infra.CrossCutting.Communication.GRPC.Protos;
using Cpnucleo.Infra.CrossCutting.Communication.GRPC.Protos.Projeto;
using Cpnucleo.Infra.CrossCutting.Util.ViewModels;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cpnucleo.GRPC
{
    public class ProjetoService : Projeto.ProjetoBase
    {
        private readonly IMapper _mapper;
        private readonly ICrudAppService<ProjetoViewModel> _projetoAppService;

        public ProjetoService(IMapper mapper, ICrudAppService<ProjetoViewModel> projetoAppService)
        {
            _mapper = mapper;
            _projetoAppService = projetoAppService;
        }

        public override async Task<BaseReply> Incluir(ProjetoModel request, ServerCallContext context)
        {
            return await Task.FromResult(new BaseReply
            {
                Id = _projetoAppService.Incluir(_mapper.Map<ProjetoViewModel>(request)).ToString()
            });
        }

        public override async Task<ListarReply> Listar(Empty request, ServerCallContext context)
        {
            ListarReply result = new ListarReply();
            result.Lista.AddRange(_mapper.Map<IEnumerable<ProjetoModel>>(_projetoAppService.Listar()));

            return await Task.FromResult(result);
        }

        public override async Task<ProjetoModel> Consultar(BaseRequest request, ServerCallContext context)
        {
            Guid id = new Guid(request.Id);
            ProjetoModel result = _mapper.Map<ProjetoModel>(_projetoAppService.Consultar(id));

            return await Task.FromResult(result);
        }

        public override async Task<BaseReply> Alterar(ProjetoModel request, ServerCallContext context)
        {
            return await Task.FromResult(new BaseReply
            {
                Sucesso = _projetoAppService.Alterar(_mapper.Map<ProjetoViewModel>(request))
            });
        }

        public override async Task<BaseReply> Remover(BaseRequest request, ServerCallContext context)
        {
            return await Task.FromResult(new BaseReply
            {
                Sucesso = _projetoAppService.Remover(new Guid(request.Id))
            });
        }
    }
}
