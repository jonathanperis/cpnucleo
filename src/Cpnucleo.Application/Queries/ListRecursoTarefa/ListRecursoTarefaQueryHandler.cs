namespace Cpnucleo.Application.Queries.ListRecursoTarefa;

public sealed class ListRecursoTarefaQueryHandler : IRequestHandler<ListRecursoTarefaQuery, ListRecursoTarefaViewModel>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public ListRecursoTarefaQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<ListRecursoTarefaViewModel> Handle(ListRecursoTarefaQuery request, CancellationToken cancellationToken)
    {
        var recursoTarefas = await _context.RecursoTarefas
            .Where(x => x.Ativo)
            .OrderBy(x => x.DataInclusao)
            .ProjectTo<RecursoTarefaDTO>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);

        if (recursoTarefas is null)
        {
            return new ListRecursoTarefaViewModel { OperationResult = OperationResult.NotFound };
        }

        return new ListRecursoTarefaViewModel { RecursoTarefas = recursoTarefas, OperationResult = OperationResult.Success };
    }
}