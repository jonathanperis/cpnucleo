namespace Cpnucleo.Application.Commands.Workflow;

public sealed class CreateWorkflowHandler : IRequestHandler<CreateWorkflowCommand, OperationResult>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CreateWorkflowHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<OperationResult> Handle(CreateWorkflowCommand request, CancellationToken cancellationToken)
    {
        await _unitOfWork.WorkflowRepository.AddAsync(_mapper.Map<Domain.Entities.Workflow>(request));

        bool success = await _unitOfWork.SaveChangesAsync();

        OperationResult result = success ? OperationResult.Success : OperationResult.Failed;

        return result;
    }
}
