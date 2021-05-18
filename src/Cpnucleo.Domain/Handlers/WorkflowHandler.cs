using Cpnucleo.Domain.Commands.Requests.Workflow;
using Cpnucleo.Domain.Commands.Responses.Workflow;
using Cpnucleo.Domain.Queries.Requests.Workflow;
using Cpnucleo.Domain.Queries.Responses.Workflow;
using Cpnucleo.Domain.UoW;
using MediatR;
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

        public WorkflowHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<CreateWorkflowResponse> Handle(CreateWorkflowComand request, CancellationToken cancellationToken)
        {
            CreateWorkflowResponse result = new CreateWorkflowResponse
            {
                Status = OperationResult.Failed
            };

            result.Workflow = await _unitOfWork.WorkflowRepository.AddAsync(request.Workflow);

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

            result.Workflow = await _unitOfWork.WorkflowRepository.GetAsync(request.Id);
            result.Status = OperationResult.Success;

            return result;
        }

        public async Task<ListWorkflowResponse> Handle(ListWorkflowQuery request, CancellationToken cancellationToken)
        {
            ListWorkflowResponse result = new ListWorkflowResponse
            {
                Status = OperationResult.Failed
            };

            result.Workflows = await _unitOfWork.WorkflowRepository.AllAsync(request.GetDependencies);
            result.Status = OperationResult.Success;

            return result;
        }

        public async Task<RemoveWorkflowResponse> Handle(RemoveWorkflowComand request, CancellationToken cancellationToken)
        {
            RemoveWorkflowResponse result = new RemoveWorkflowResponse
            {
                Status = OperationResult.Failed
            };

            Entities.Workflow obj = await _unitOfWork.WorkflowRepository.GetAsync(request.Id);

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

            _unitOfWork.WorkflowRepository.Update(request.Workflow);

            bool success = await _unitOfWork.SaveChangesAsync();

            result.Status = success ? OperationResult.Success : OperationResult.Failed;

            return result;
        }
    }
}
