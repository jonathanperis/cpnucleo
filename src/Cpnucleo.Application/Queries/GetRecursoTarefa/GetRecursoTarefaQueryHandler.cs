namespace Cpnucleo.Application.Queries.GetRecursoTarefa;

public sealed class GetRecursoTarefaQueryHandler : IRequestHandler<GetRecursoTarefaQuery, GetRecursoTarefaViewModel>
{
    private readonly IApplicationDbContext _context;

    public GetRecursoTarefaQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async ValueTask<GetRecursoTarefaViewModel> Handle(GetRecursoTarefaQuery request, CancellationToken cancellationToken)
    {
        var recursoTarefa = await _context.RecursoTarefas
            .AsNoTracking()
            .Include(x => x.Recurso)
            .Include(x => x.Tarefa)
            .Where(x => x.Id == request.Id && x.Ativo)
            .Select(x => x.MapToDto())
            .FirstOrDefaultAsync(cancellationToken);

        if (recursoTarefa is null)
        {
            return new GetRecursoTarefaViewModel { OperationResult = OperationResult.NotFound };
        }

        return new GetRecursoTarefaViewModel { RecursoTarefa = recursoTarefa, OperationResult = OperationResult.Success };
    }
}