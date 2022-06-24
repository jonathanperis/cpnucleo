using Cpnucleo.Shared.Commands.Workflow;
using Cpnucleo.Shared.Common.Models;

namespace Cpnucleo.Application.Commands.Workflow;

public class RemoveWorkflowHandler : IRequestHandler<RemoveWorkflowCommand, OperationResult>
{
    private readonly IUnitOfWork _unitOfWork;

    public RemoveWorkflowHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<OperationResult> Handle(RemoveWorkflowCommand request, CancellationToken cancellationToken)
    {
        Domain.Entities.Workflow workflow = await _unitOfWork.WorkflowRepository.GetAsync(request.Id);

        if (workflow == null)
        {
            return OperationResult.NotFound;
        }

        await _unitOfWork.WorkflowRepository.RemoveAsync(request.Id);

        bool success = await _unitOfWork.SaveChangesAsync();

        OperationResult result = success ? OperationResult.Success : OperationResult.Failed;

        return result;
    }
}
