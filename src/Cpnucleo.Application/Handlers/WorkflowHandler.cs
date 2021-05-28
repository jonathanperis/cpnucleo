using AutoMapper;
using Cpnucleo.Domain.Entities;
using Cpnucleo.Domain.UoW;
using Cpnucleo.Infra.CrossCutting.Util;
using Cpnucleo.Infra.CrossCutting.Util.Commands.Requests.Workflow;
using Cpnucleo.Infra.CrossCutting.Util.Commands.Responses.Workflow;
using Cpnucleo.Infra.CrossCutting.Util.Queries.Requests.Workflow;
using Cpnucleo.Infra.CrossCutting.Util.Queries.Responses.Workflow;
using Cpnucleo.Infra.CrossCutting.Util.ViewModels;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Cpnucleo.Domain.Handlers
{
    internal class WorkflowHandler :
        IRequestHandler<CreateWorkflowComand, CreateWorkflowResponse>,
        IRequestHandler<GetWorkflowQuery, GetWorkflowResponse>,
        IRequestHandler<ListWorkflowQuery, ListWorkflowResponse>,
        IRequestHandler<RemoveWorkflowComand, RemoveWorkflowResponse>,
        IRequestHandler<UpdateWorkflowComand, UpdateWorkflowResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public WorkflowHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<CreateWorkflowResponse> Handle(CreateWorkflowComand request, CancellationToken cancellationToken)
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

        public async Task<GetWorkflowResponse> Handle(GetWorkflowQuery request, CancellationToken cancellationToken)
        {
            GetWorkflowResponse result = new GetWorkflowResponse
            {
                Status = OperationResult.Failed
            };

            result.Workflow = _mapper.Map<WorkflowViewModel>(await _unitOfWork.WorkflowRepository.GetAsync(request.Id));
            result.Status = OperationResult.Success;

            return result;
        }

        public async Task<ListWorkflowResponse> Handle(ListWorkflowQuery request, CancellationToken cancellationToken)
        {
            ListWorkflowResponse result = new ListWorkflowResponse
            {
                Status = OperationResult.Failed
            };

            result.Workflows = _mapper.Map<IEnumerable<WorkflowViewModel>>(await _unitOfWork.WorkflowRepository.AllAsync(request.GetDependencies));
            result.Status = OperationResult.Success;

            return result;
        }

        public async Task<RemoveWorkflowResponse> Handle(RemoveWorkflowComand request, CancellationToken cancellationToken)
        {
            RemoveWorkflowResponse result = new RemoveWorkflowResponse
            {
                Status = OperationResult.Failed
            };

            Workflow obj = await _unitOfWork.WorkflowRepository.GetAsync(request.Id);

            if (obj == null)
            {
                return null;
            }

            await _unitOfWork.WorkflowRepository.RemoveAsync(request.Id);

            bool success = await _unitOfWork.SaveChangesAsync();

            result.Status = success ? OperationResult.Success : OperationResult.Failed;

            return result;
        }

        public async Task<UpdateWorkflowResponse> Handle(UpdateWorkflowComand request, CancellationToken cancellationToken)
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
    }
}
