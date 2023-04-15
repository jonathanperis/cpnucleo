namespace Cpnucleo.Application.Queries.GetRecursoTarefa;

public sealed class GetRecursoTarefaQueryHandler : IRequestHandler<GetRecursoTarefaQuery, GetRecursoTarefaViewModel>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetRecursoTarefaQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async ValueTask<GetRecursoTarefaViewModel> Handle(GetRecursoTarefaQuery request, CancellationToken cancellationToken)
    {
        var recursoTarefa = await _context.RecursoTarefas
            .Where(x => x.Id == request.Id && x.Ativo)
            .ProjectTo<RecursoTarefaDTO>(_mapper.ConfigurationProvider)
            .FirstOrDefaultAsync(cancellationToken);

        if (recursoTarefa is null)
        {
            return new GetRecursoTarefaViewModel { OperationResult = OperationResult.NotFound };
        }

        return new GetRecursoTarefaViewModel { RecursoTarefa = recursoTarefa, OperationResult = OperationResult.Success };
    }
}