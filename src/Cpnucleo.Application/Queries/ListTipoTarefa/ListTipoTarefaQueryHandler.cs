namespace Cpnucleo.Application.Queries.ListTipoTarefa;

public sealed class ListTipoTarefaQueryHandler : IRequestHandler<ListTipoTarefaQuery, ListTipoTarefaViewModel>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public ListTipoTarefaQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<ListTipoTarefaViewModel> Handle(ListTipoTarefaQuery request, CancellationToken cancellationToken)
    {
        var tipoTarefas = await _context.TipoTarefas
            .Where(x => x.Ativo)
            .OrderBy(x => x.DataInclusao)
            .ProjectTo<TipoTarefaDTO>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);

        if (tipoTarefas is null)
        {
            return new ListTipoTarefaViewModel { OperationResult = OperationResult.NotFound };
        }

        return new ListTipoTarefaViewModel { TipoTarefas = tipoTarefas, OperationResult = OperationResult.Success };
    }
}