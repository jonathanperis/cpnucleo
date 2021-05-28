using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cpnucleo.Infra.CrossCutting.Util;
using Cpnucleo.Infra.CrossCutting.Util.ViewModels;
using Cpnucleo.Application.Interfaces;
using ProtoBuf.Grpc;
using System.Linq;

namespace Cpnucleo.GRPC
{
    //[Authorize]    
    public class SistemaService : ISistemaGrpcService
    {
        private readonly ISistemaAppService _sistemaAppService;

        public SistemaService(ISistemaAppService sistemaAppService)
        {
            _sistemaAppService = sistemaAppService;
        }

        public async Task<SistemaViewModel> AddAsync(SistemaViewModel viewModel, CallContext context = default)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<SistemaViewModel>> AllAsync(IEnumerable<SistemaViewModel> viewModels, CallContext context = default)
        {
            var a = viewModels.FirstOrDefault();

            await _sistemaAppService.AddAsync(a);

            throw new NotImplementedException();
        }

        public async Task<SistemaViewModel> GetAsync(string id, CallContext context = default)
        {
            throw new NotImplementedException();
        }

        public async Task RemoveAsync(string id, CallContext context = default)
        {
            throw new NotImplementedException();
        }

        public void Update(SistemaViewModel viewModel, CallContext context = default)
        {
            throw new NotImplementedException();
        }
    }
}
