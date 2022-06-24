﻿using Cpnucleo.Shared.Commands.Workflow;
using Cpnucleo.Shared.Common.Models;

namespace Cpnucleo.Application.Commands.Workflow;

public class UpdateWorkflowHandler : IRequestHandler<UpdateWorkflowCommand, OperationResult>
{
    private readonly IUnitOfWork _unitOfWork;

    public UpdateWorkflowHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<OperationResult> Handle(UpdateWorkflowCommand request, CancellationToken cancellationToken)
    {
        Domain.Entities.Workflow workflow = await _unitOfWork.WorkflowRepository.GetAsync(request.Id);

        if (workflow == null)
        {
            return OperationResult.NotFound;
        }

        workflow.Nome = request.Nome;
        workflow.Ordem = request.Ordem;

        _unitOfWork.WorkflowRepository.Update(workflow);

        bool success = await _unitOfWork.SaveChangesAsync();

        OperationResult result = success ? OperationResult.Success : OperationResult.Failed;

        return result;
    }
}
