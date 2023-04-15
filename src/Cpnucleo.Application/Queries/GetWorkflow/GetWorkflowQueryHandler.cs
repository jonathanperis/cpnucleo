namespace Cpnucleo.Application.Queries.GetWorkflow;

public sealed class GetWorkflowQueryHandler : IRequestHandler<GetWorkflowQuery, GetWorkflowViewModel>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetWorkflowQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async ValueTask<GetWorkflowViewModel> Handle(GetWorkflowQuery request, CancellationToken cancellationToken)
    {
        var workflow = await _context.Workflows
            .Where(x => x.Id == request.Id && x.Ativo)
            .ProjectTo<WorkflowDTO>(_mapper.ConfigurationProvider)
            .FirstOrDefaultAsync(cancellationToken);

        if (workflow is null)
        {
            return new GetWorkflowViewModel { OperationResult = OperationResult.NotFound };
        }

        return new GetWorkflowViewModel { Workflow = workflow, OperationResult = OperationResult.Success };
    }
}