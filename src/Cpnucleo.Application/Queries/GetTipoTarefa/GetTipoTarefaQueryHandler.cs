namespace Cpnucleo.Application.Queries.GetTipoTarefa;

public sealed class GetTipoTarefaQueryHandler : IRequestHandler<GetTipoTarefaQuery, GetTipoTarefaViewModel>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetTipoTarefaQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async ValueTask<GetTipoTarefaViewModel> Handle(GetTipoTarefaQuery request, CancellationToken cancellationToken)
    {
        var tipoTarefa = await _context.TipoTarefas
            .Where(x => x.Id == request.Id && x.Ativo)
            .ProjectTo<TipoTarefaDTO>(_mapper.ConfigurationProvider)
            .FirstOrDefaultAsync(cancellationToken);

        if (tipoTarefa is null)
        {
            return new GetTipoTarefaViewModel { OperationResult = OperationResult.NotFound };
        }

        return new GetTipoTarefaViewModel { TipoTarefa = tipoTarefa, OperationResult = OperationResult.Success };
    }
}