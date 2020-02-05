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
    public class TipoTarefaService : TipoTarefa.TipoTarefaBase
    {
        private readonly IMapper _mapper;
        private readonly ITipoTarefaAppService _tipoTarefaAppService;

        public TipoTarefaService(IMapper mapper, ITipoTarefaAppService tipoTarefaAppService)
        {
            _mapper = mapper;
            _tipoTarefaAppService = tipoTarefaAppService;
        }

        //bool Incluir(TViewModel obj);
        public override async Task<BaseReply> Incluir(TipoTarefaModel request, ServerCallContext context)
        {
            //if (!ModelState.IsValid)
            //{
            //    return BadRequest(ModelState);
            //}

            //try
            //{
            //    _tipoTarefaAppService.Incluir(obj);
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
                Sucesso = _tipoTarefaAppService.Incluir(_mapper.Map<TipoTarefaViewModel>(request))
            });
        }

        //IEnumerable<TViewModel> Listar();
        public override async Task Listar(Empty request, IServerStreamWriter<TipoTarefaModel> responseStream, ServerCallContext context)
        {
            //return _tipoTarefaAppService.Listar();

            foreach (TipoTarefaModel item in _mapper.Map<IEnumerable<TipoTarefaModel>>(_tipoTarefaAppService.Listar()))
            {
                await responseStream.WriteAsync(item);
            }
        }

        //TViewModel Consultar(Guid id);
        public override async Task<TipoTarefaModel> Consultar(BaseRequest request, ServerCallContext context)
        {
            //TipoTarefaViewModel tipoTarefa = _tipoTarefaAppService.Consultar(id);

            //if (tipoTarefa == null)
            //{
            //    return NotFound();
            //}

            //return Ok(tipoTarefa);

            Guid id = new Guid(request.Id);
            TipoTarefaModel result = _mapper.Map<TipoTarefaModel>(_tipoTarefaAppService.Consultar(id));

            return await Task.FromResult(result);
        }

        //bool Alterar(TViewModel obj);
        public override async Task<BaseReply> Alterar(TipoTarefaModel request, ServerCallContext context)
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
            //    _tipoTarefaAppService.Alterar(obj);
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
                Sucesso = _tipoTarefaAppService.Alterar(_mapper.Map<TipoTarefaViewModel>(request))
            });
        }

        //bool Remover(Guid id);
        public override async Task<BaseReply> Remover(BaseRequest request, ServerCallContext context)
        {
            //TipoTarefaViewModel obj = _tipoTarefaAppService.Consultar(id);

            //if (obj == null)
            //{
            //    return NotFound();
            //}

            //_tipoTarefaAppService.Remover(id);

            //return NoContent();

            return await Task.FromResult(new BaseReply
            {
                Sucesso = _tipoTarefaAppService.Remover(new Guid(request.Id))
            });
        }
    }
}
