namespace Cpnucleo.Application.Queries.ListWorkflow;

public sealed class ListWorkflowQueryHandler : IRequestHandler<ListWorkflowQuery, ListWorkflowViewModel>
{
    private readonly IApplicationDbContext _context;

    public ListWorkflowQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async ValueTask<ListWorkflowViewModel> Handle(ListWorkflowQuery request, CancellationToken cancellationToken)
    {
        var workflows = await _context.Workflows
            .AsNoTracking()
            .Where(x => x.Ativo)
            .OrderBy(x => x.DataInclusao)
            .Select(x => x.MapToDto())
            .ToListAsync(cancellationToken);

        if (workflows is null)
        {
            return new ListWorkflowViewModel { OperationResult = OperationResult.NotFound };
        }

        var colunas = _context.Workflows.Where(x => x.Ativo).Count();

        workflows.ForEach(x => x.TamanhoColuna = Workflow.GetTamanhoColuna(colunas));

        return new ListWorkflowViewModel { Workflows = workflows, OperationResult = OperationResult.Success };
    }
}