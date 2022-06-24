using Cpnucleo.Shared.Common.DTOs;
using Cpnucleo.Shared.Common.Models;
using Cpnucleo.Shared.Queries.Workflow;

namespace Cpnucleo.Application.Queries.Workflow;

public class GetWorkflowHandler : IRequestHandler<GetWorkflowQuery, GetWorkflowViewModel>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetWorkflowHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<GetWorkflowViewModel> Handle(GetWorkflowQuery request, CancellationToken cancellationToken)
    {
        Domain.Entities.Workflow workflow = await _unitOfWork.WorkflowRepository.GetAsync(request.Id);

        if (workflow == null)
        {
            return new GetWorkflowViewModel { OperationResult = OperationResult.NotFound };
        }

        WorkflowDTO result = _mapper.Map<WorkflowDTO>(workflow);

        return new GetWorkflowViewModel { Workflow = result, OperationResult = OperationResult.Success };
    }
}
