using AutoMapper;
using Cpnucleo.Domain.Entities;
using Cpnucleo.Domain.UoW;
using Cpnucleo.Infra.CrossCutting.Util;
using Cpnucleo.Infra.CrossCutting.Util.Commands.Workflow.CreateWorkflow;
using Cpnucleo.Infra.CrossCutting.Util.Commands.Workflow.RemoveWorkflow;
using Cpnucleo.Infra.CrossCutting.Util.Commands.Workflow.UpdateWorkflow;
using Cpnucleo.Infra.CrossCutting.Util.Queries.Workflow.GetWorkflow;
using Cpnucleo.Infra.CrossCutting.Util.Queries.Workflow.ListWorkflow;
using Cpnucleo.Infra.CrossCutting.Util.ViewModels;
using MessagePipe;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Cpnucleo.Application.Handlers
{
    public class WorkflowHandler :
        IAsyncRequestHandler<CreateWorkflowCommand, CreateWorkflowResponse>,
        IAsyncRequestHandler<GetWorkflowQuery, GetWorkflowResponse>,
        IAsyncRequestHandler<ListWorkflowQuery, ListWorkflowResponse>,
        IAsyncRequestHandler<RemoveWorkflowCommand, RemoveWorkflowResponse>,
        IAsyncRequestHandler<UpdateWorkflowCommand, UpdateWorkflowResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public WorkflowHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async ValueTask<CreateWorkflowResponse> InvokeAsync(CreateWorkflowCommand request, CancellationToken cancellationToken)
        {
            CreateWorkflowResponse result = new CreateWorkflowResponse
            {
                Status = OperationResult.Failed
            };

            Workflow obj = await _unitOfWork.WorkflowRepository.AddAsync(_mapper.Map<Workflow>(request.Workflow));
            result.Workflow = _mapper.Map<WorkflowViewModel>(obj);

            await _unitOfWork.SaveChangesAsync();

            result.Status = OperationResult.Success;

            return result;
        }

        public async ValueTask<GetWorkflowResponse> InvokeAsync(GetWorkflowQuery request, CancellationToken cancellationToken)
        {
            GetWorkflowResponse result = new GetWorkflowResponse
            {
                Status = OperationResult.Failed
            };

            result.Workflow = _mapper.Map<WorkflowViewModel>(await _unitOfWork.WorkflowRepository.GetAsync(request.Id));

            if (result.Workflow == null)
            {
                result.Status = OperationResult.NotFound;

                return result;
            }

            result.Status = OperationResult.Success;

            return result;
        }

        public async ValueTask<ListWorkflowResponse> InvokeAsync(ListWorkflowQuery request, CancellationToken cancellationToken)
        {
            ListWorkflowResponse result = new ListWorkflowResponse
            {
                Status = OperationResult.Failed
            };

            result.Workflows = _mapper.Map<IEnumerable<WorkflowViewModel>>(await _unitOfWork.WorkflowRepository.AllAsync(request.GetDependencies));
            result.Status = OperationResult.Success;

            await PreencherDadosAdicionaisAsync(result.Workflows);

            return result;
        }

        public async ValueTask<RemoveWorkflowResponse> InvokeAsync(RemoveWorkflowCommand request, CancellationToken cancellationToken)
        {
            RemoveWorkflowResponse result = new RemoveWorkflowResponse
            {
                Status = OperationResult.Failed
            };

            Workflow obj = await _unitOfWork.WorkflowRepository.GetAsync(request.Id);

            if (obj == null)
            {
                result.Status = OperationResult.NotFound;

                return result;
            }

            await _unitOfWork.WorkflowRepository.RemoveAsync(request.Id);

            bool success = await _unitOfWork.SaveChangesAsync();

            result.Status = success ? OperationResult.Success : OperationResult.Failed;

            return result;
        }

        public async ValueTask<UpdateWorkflowResponse> InvokeAsync(UpdateWorkflowCommand request, CancellationToken cancellationToken)
        {
            UpdateWorkflowResponse result = new UpdateWorkflowResponse
            {
                Status = OperationResult.Failed
            };

            _unitOfWork.WorkflowRepository.Update(_mapper.Map<Workflow>(request.Workflow));

            bool success = await _unitOfWork.SaveChangesAsync();

            result.Status = success ? OperationResult.Success : OperationResult.Failed;

            return result;
        }

        private async Task PreencherDadosAdicionaisAsync(IEnumerable<WorkflowViewModel> lista)
        {
            int colunas = await _unitOfWork.WorkflowRepository.GetQuantidadeColunasAsync();

            foreach (WorkflowViewModel item in lista)
            {
                item.TamanhoColuna = _unitOfWork.WorkflowRepository.GetTamanhoColuna(colunas);
            }
        }
    }
}
