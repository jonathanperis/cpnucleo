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
    public class SistemaService : Sistema.SistemaBase
    {
        private readonly IMapper _mapper;
        private readonly ICrudAppService<SistemaViewModel> _sistemaAppService;

        public SistemaService(IMapper mapper, ICrudAppService<SistemaViewModel> sistemaAppService)
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
        public override async Task<SistemaModel> Consultar(BaseRequest request, ServerCallContext context)
        {
            //SistemaViewModel sistema = _sistemaAppService.Consultar(id);

            //if (sistema == null)
            //{
            //    return NotFound();
            //}

            //return Ok(sistema);

            Guid id = new Guid(request.Id);
            SistemaModel result = _mapper.Map<SistemaModel>(_sistemaAppService.Consultar(id));

            return await Task.FromResult(result);
        }

        //bool Alterar(TViewModel obj);
        public override async Task<BaseReply> Alterar(SistemaModel request, ServerCallContext context)
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

            return await Task.FromResult(new BaseReply
            {
                Sucesso = _sistemaAppService.Alterar(_mapper.Map<SistemaViewModel>(request))
            });
        }

        //bool Remover(Guid id);
        public override async Task<BaseReply> Remover(BaseRequest request, ServerCallContext context)
        {
            //SistemaViewModel obj = _sistemaAppService.Consultar(id);

            //if (obj == null)
            //{
            //    return NotFound();
            //}

            //_sistemaAppService.Remover(id);

            //return NoContent();

            return await Task.FromResult(new BaseReply
            {
                Sucesso = _sistemaAppService.Remover(new Guid(request.Id))
            });
        }
    }
}
