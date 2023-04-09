namespace Cpnucleo.Application.Queries.ListRecursoTarefaByTarefa;

public sealed class ListRecursoTarefaByTarefaQueryHandler : IRequestHandler<ListRecursoTarefaByTarefaQuery, ListRecursoTarefaByTarefaViewModel>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public ListRecursoTarefaByTarefaQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<ListRecursoTarefaByTarefaViewModel> Handle(ListRecursoTarefaByTarefaQuery request, CancellationToken cancellationToken)
    {
        var recursoTarefas = await _context.RecursoTarefas
            .Where(x => x.IdTarefa == request.IdTarefa && x.Ativo)
            .OrderBy(x => x.DataInclusao)
            .ProjectTo<RecursoTarefaDTO>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);

        if (recursoTarefas is null)
        {
            return new ListRecursoTarefaByTarefaViewModel { OperationResult = OperationResult.NotFound };
        }

        return new ListRecursoTarefaByTarefaViewModel { RecursoTarefas = recursoTarefas, OperationResult = OperationResult.Success };
    }
}