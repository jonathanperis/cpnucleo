using AutoMapper;
using Cpnucleo.Application.Interfaces;
using Cpnucleo.Infra.CrossCutting.Util.ViewModels;
using Google.Protobuf.WellKnownTypes;
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
        public override async Task<BaseReply> Incluir(SistemaModel request, ServerCallContext context)
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

            return await Task.FromResult(new BaseReply
            {
                Sucesso = _sistemaAppService.Incluir(_mapper.Map<SistemaViewModel>(request))
            });
        }

        //IEnumerable<TViewModel> Listar();
        public override async Task Listar(Empty request, IServerStreamWriter<SistemaModel> responseStream, ServerCallContext context)
        {
            //return _sistemaAppService.Listar();

            foreach (SistemaModel item in _mapper.Map<IEnumerable<SistemaModel>>(_sistemaAppService.Listar()))
            {
                await responseStream.WriteAsync(item);
            }
        }

        //TViewModel Consultar(Guid id);
        public override Task<SistemaModel> Consultar(BaseRequest request, ServerCallContext context)
        {
            //SistemaViewModel sistema = _sistemaAppService.Consultar(id);

            //if (sistema == null)
            //{
            //    return NotFound();
            //}

            //return Ok(sistema);

            return base.Consultar(request, context);
        }

        //bool Alterar(TViewModel obj);
        public override Task<BaseReply> Alterar(SistemaModel request, ServerCallContext context)
        {
            //if (!ModelState.IsValid)
            //{
            //    return BadRequest(ModelState);
            //}

            //if (id != obj.Id)
            //{
            //    return BadRequest();
            //}

            //try
            //{
            //    _sistemaAppService.Alterar(obj);
            //}
            //catch (DbUpdateConcurrencyException)
            //{
            //    if (!ObjExists(id))
            //    {
            //        return NotFound();
            //    }
            //    else
            //    {
            //        throw;
            //    }
            //}

            //return NoContent();

            return base.Alterar(request, context);
        }

        //bool Remover(Guid id);
        public override Task<BaseReply> Remover(BaseRequest request, ServerCallContext context)
        {
            //SistemaViewModel obj = _sistemaAppService.Consultar(id);

            //if (obj == null)
            //{
            //    return NotFound();
            //}

            //_sistemaAppService.Remover(id);

            //return NoContent();

            return base.Remover(request, context);
        }
    }
}
