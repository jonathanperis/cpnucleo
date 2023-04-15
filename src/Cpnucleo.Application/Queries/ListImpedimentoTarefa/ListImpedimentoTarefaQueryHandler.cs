namespace Cpnucleo.Application.Queries.ListImpedimentoTarefa;

public sealed class ListImpedimentoTarefaQueryHandler : IRequestHandler<ListImpedimentoTarefaQuery, ListImpedimentoTarefaViewModel>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public ListImpedimentoTarefaQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async ValueTask<ListImpedimentoTarefaViewModel> Handle(ListImpedimentoTarefaQuery request, CancellationToken cancellationToken)
    {
        var impedimentoTarefas = await _context.ImpedimentoTarefas
            .Where(x => x.Ativo)
            .OrderBy(x => x.DataInclusao)
            .ProjectTo<ImpedimentoTarefaDTO>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);

        if (impedimentoTarefas is null)
        {
            return new ListImpedimentoTarefaViewModel { OperationResult = OperationResult.NotFound };
        }

        return new ListImpedimentoTarefaViewModel { ImpedimentoTarefas = impedimentoTarefas, OperationResult = OperationResult.Success };
    }
}