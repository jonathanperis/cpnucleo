namespace Cpnucleo.Application.Queries.ListWorkflow;

public sealed class ListWorkflowQueryHandler : IRequestHandler<ListWorkflowQuery, ListWorkflowViewModel>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public ListWorkflowQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<ListWorkflowViewModel> Handle(ListWorkflowQuery request, CancellationToken cancellationToken)
    {
        var workflows = await _context.Workflows
            .Where(x => x.Ativo)
            .OrderBy(x => x.DataInclusao)
            .ProjectTo<WorkflowDTO>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);

        if (workflows is null)
        {
            return new ListWorkflowViewModel { OperationResult = OperationResult.NotFound };
        }

        return new ListWorkflowViewModel { Workflows = workflows, OperationResult = OperationResult.Success };
    }
}