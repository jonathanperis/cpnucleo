namespace Cpnucleo.Application.Queries.GetWorkflow;

public sealed class GetWorkflowQueryHandler : IRequestHandler<GetWorkflowQuery, GetWorkflowViewModel>
{
    private readonly IApplicationDbContext _context;

    public GetWorkflowQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async ValueTask<GetWorkflowViewModel> Handle(GetWorkflowQuery request, CancellationToken cancellationToken)
    {
        var workflow = await _context.Workflows
            .AsNoTracking()
            .Where(x => x.Id == request.Id && x.Ativo)
            .Select(x => x.MapToDto())
            .FirstOrDefaultAsync(cancellationToken);

        if (workflow is null)
        {
            return new GetWorkflowViewModel { OperationResult = OperationResult.NotFound };
        }

        return new GetWorkflowViewModel { Workflow = workflow, OperationResult = OperationResult.Success };
    }
}