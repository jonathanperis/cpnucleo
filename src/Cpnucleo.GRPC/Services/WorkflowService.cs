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
    public class WorkflowService : Workflow.WorkflowBase
    {
        private readonly IMapper _mapper;
        private readonly IWorkflowAppService _workflowAppService;

        public WorkflowService(IMapper mapper, IWorkflowAppService workflowAppService)
        {
            _mapper = mapper;
            _workflowAppService = workflowAppService;
        }

        //bool Incluir(TViewModel obj);
        public override async Task<BaseReply> Incluir(WorkflowModel request, ServerCallContext context)
        {
            //if (!ModelState.IsValid)
            //{
            //    return BadRequest(ModelState);
            //}

            //try
            //{
            //    _workflowAppService.Incluir(obj);
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
                Sucesso = _workflowAppService.Incluir(_mapper.Map<WorkflowViewModel>(request))
            });
        }

        //IEnumerable<TViewModel> Listar();
        public override async Task Listar(Empty request, IServerStreamWriter<WorkflowModel> responseStream, ServerCallContext context)
        {
            //return _workflowAppService.Listar();

            foreach (WorkflowModel item in _mapper.Map<IEnumerable<WorkflowModel>>(_workflowAppService.Listar()))
            {
                await responseStream.WriteAsync(item);
            }
        }

        //TViewModel Consultar(Guid id);
        public override async Task<WorkflowModel> Consultar(BaseRequest request, ServerCallContext context)
        {
            //WorkflowViewModel workflow = _workflowAppService.Consultar(id);

            //if (workflow == null)
            //{
            //    return NotFound();
            //}

            //return Ok(workflow);

            Guid id = new Guid(request.Id);
            WorkflowModel result = _mapper.Map<WorkflowModel>(_workflowAppService.Consultar(id));

            return await Task.FromResult(result);
        }

        //bool Alterar(TViewModel obj);
        public override async Task<BaseReply> Alterar(WorkflowModel request, ServerCallContext context)
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
            //    _workflowAppService.Alterar(obj);
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
                Sucesso = _workflowAppService.Alterar(_mapper.Map<WorkflowViewModel>(request))
            });
        }

        //bool Remover(Guid id);
        public override async Task<BaseReply> Remover(BaseRequest request, ServerCallContext context)
        {
            //WorkflowViewModel obj = _workflowAppService.Consultar(id);

            //if (obj == null)
            //{
            //    return NotFound();
            //}

            //_workflowAppService.Remover(id);

            //return NoContent();

            return await Task.FromResult(new BaseReply
            {
                Sucesso = _workflowAppService.Remover(new Guid(request.Id))
            });
        }
    }
}
