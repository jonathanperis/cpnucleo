namespace Cpnucleo.Application.Queries.Workflow;

public sealed class GetWorkflowHandler : IRequestHandler<GetWorkflowQuery, GetWorkflowViewModel>
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
        WorkflowDTO workflow = await _unitOfWork.WorkflowRepository.Get(request.Id)
            .ProjectTo<WorkflowDTO>(_mapper.ConfigurationProvider)
            .FirstOrDefaultAsync(cancellationToken);

        if (workflow is null)
        {
            return new GetWorkflowViewModel { OperationResult = OperationResult.NotFound };
        }

        return new GetWorkflowViewModel { Workflow = workflow, OperationResult = OperationResult.Success };
    }
}
