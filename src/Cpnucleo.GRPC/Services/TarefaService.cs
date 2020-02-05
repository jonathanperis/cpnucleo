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
    public class TarefaService : Tarefa.TarefaBase
    {
        private readonly IMapper _mapper;
        private readonly ITarefaAppService _tarefaAppService;

        public TarefaService(IMapper mapper, ITarefaAppService tarefaAppService)
        {
            _mapper = mapper;
            _tarefaAppService = tarefaAppService;
        }

        //bool Incluir(TViewModel obj);
        public override async Task<BaseReply> Incluir(TarefaModel request, ServerCallContext context)
        {
            //if (!ModelState.IsValid)
            //{
            //    return BadRequest(ModelState);
            //}

            //try
            //{
            //    _tarefaAppService.Incluir(obj);
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
                Sucesso = _tarefaAppService.Incluir(_mapper.Map<TarefaViewModel>(request))
            });
        }

        //IEnumerable<TViewModel> Listar();
        public override async Task Listar(Empty request, IServerStreamWriter<TarefaModel> responseStream, ServerCallContext context)
        {
            //return _tarefaAppService.Listar();

            foreach (TarefaModel item in _mapper.Map<IEnumerable<TarefaModel>>(_tarefaAppService.Listar()))
            {
                await responseStream.WriteAsync(item);
            }
        }

        //TViewModel Consultar(Guid id);
        public override async Task<TarefaModel> Consultar(BaseRequest request, ServerCallContext context)
        {
            //TarefaViewModel tarefa = _tarefaAppService.Consultar(id);

            //if (tarefa == null)
            //{
            //    return NotFound();
            //}

            //return Ok(tarefa);

            Guid id = new Guid(request.Id);
            TarefaModel result = _mapper.Map<TarefaModel>(_tarefaAppService.Consultar(id));

            return await Task.FromResult(result);
        }

        //bool Alterar(TViewModel obj);
        public override async Task<BaseReply> Alterar(TarefaModel request, ServerCallContext context)
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
            //    _tarefaAppService.Alterar(obj);
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
                Sucesso = _tarefaAppService.Alterar(_mapper.Map<TarefaViewModel>(request))
            });
        }

        //bool Remover(Guid id);
        public override async Task<BaseReply> Remover(BaseRequest request, ServerCallContext context)
        {
            //TarefaViewModel obj = _tarefaAppService.Consultar(id);

            //if (obj == null)
            //{
            //    return NotFound();
            //}

            //_tarefaAppService.Remover(id);

            //return NoContent();

            return await Task.FromResult(new BaseReply
            {
                Sucesso = _tarefaAppService.Remover(new Guid(request.Id))
            });
        }
    }
}
