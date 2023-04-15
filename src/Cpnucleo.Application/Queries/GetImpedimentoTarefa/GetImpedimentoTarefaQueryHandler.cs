namespace Cpnucleo.Application.Queries.GetImpedimentoTarefa;

public sealed class GetImpedimentoTarefaQueryHandler : IRequestHandler<GetImpedimentoTarefaQuery, GetImpedimentoTarefaViewModel>
{
    private readonly IApplicationDbContext _context;

    public GetImpedimentoTarefaQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async ValueTask<GetImpedimentoTarefaViewModel> Handle(GetImpedimentoTarefaQuery request, CancellationToken cancellationToken)
    {
        var impedimentoTarefa = await _context.ImpedimentoTarefas
            .AsNoTracking()
            .Include(x => x.Tarefa)
            .Where(x => x.Id == request.Id && x.Ativo)
            .Select(x => x.MapToDto())
            .FirstOrDefaultAsync(cancellationToken);

        if (impedimentoTarefa is null)
        {
            return new GetImpedimentoTarefaViewModel { OperationResult = OperationResult.NotFound };
        }

        return new GetImpedimentoTarefaViewModel { ImpedimentoTarefa = impedimentoTarefa, OperationResult = OperationResult.Success };
    }
}