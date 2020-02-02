using AutoMapper;
using Cpnucleo.Application.Interfaces;
using Cpnucleo.Infra.CrossCutting.Util.ViewModels;
using Grpc.Core;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cpnucleo.GRPC
{
    public class SistemaService : Sistema.SistemaBase
    {
        private readonly IMapper _mapper;
        private readonly ISistemaAppService _sistemaAppService;

        public SistemaService(IMapper mapper, ISistemaAppService sistemaAppService)
        {
            _mapper = mapper;
            _sistemaAppService = sistemaAppService;
        }

        //bool Incluir(TViewModel obj);
        public override async Task<IncluirReply> Incluir(SistemaModel request, ServerCallContext context)
        {
            //if (!ModelState.IsValid)
            //{
            //    return BadRequest(ModelState);
            //}

            //try
            //{
            //    _sistemaAppService.Incluir(obj);
            //}
            //catch (DbUpdateException)
            //{
            //    if (ObjExists(obj.Id))
            //    {
            //        return Conflict();
            //    }
            //    else
            //    {
            //        throw;
            //    }
            //}

            //return CreatedAtAction(nameof(Get), new { id = obj.Id }, obj);

            return await Task.FromResult(new IncluirReply
            {
                Sucesso = _sistemaAppService.Incluir(_mapper.Map<SistemaViewModel>(request))
            });
        }

        //IEnumerable<TViewModel> Listar();
        public override async Task Listar(ListarRequest request, IServerStreamWriter<SistemaModel> responseStream, ServerCallContext context)
        {
            //return _sistemaAppService.Listar();

            foreach (SistemaModel item in _mapper.Map<IEnumerable<SistemaModel>>(_sistemaAppService.Listar()))
            {
                await responseStream.WriteAsync(item);
            }
        }

        //TViewModel Consultar(Guid id);

        //bool Alterar(TViewModel obj);

        //bool Remover(Guid id);
    }
}
