namespace Cpnucleo.Application.Queries.GetTarefa;

public sealed class GetTarefaQueryHandler : IRequestHandler<GetTarefaQuery, GetTarefaViewModel>
{
    private readonly IApplicationDbContext _context;

    public GetTarefaQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async ValueTask<GetTarefaViewModel> Handle(GetTarefaQuery request, CancellationToken cancellationToken)
    {
        var tarefa = await _context.Tarefas
            .AsNoTracking()
            .Include(x => x.Projeto)
            .Include(x => x.Recurso)
            .Include(x => x.Workflow)
            .Include(x => x.TipoTarefa)
            .Where(x => x.Id == request.Id && x.Ativo)
            .Select(x => x.MapToDto())
            .FirstOrDefaultAsync(cancellationToken);

        if (tarefa is null)
        {
            return new GetTarefaViewModel { OperationResult = OperationResult.NotFound };
        }

        return new GetTarefaViewModel { Tarefa = tarefa, OperationResult = OperationResult.Success };
    }
}