namespace Cpnucleo.Application.Queries.GetImpedimentoTarefa;

public sealed class GetImpedimentoTarefaQueryHandler : IRequestHandler<GetImpedimentoTarefaQuery, GetImpedimentoTarefaViewModel>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetImpedimentoTarefaQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<GetImpedimentoTarefaViewModel> Handle(GetImpedimentoTarefaQuery request, CancellationToken cancellationToken)
    {
        var impedimentoTarefa = await _context.ImpedimentoTarefas
            .Where(x => x.Id == request.Id && x.Ativo)
            .ProjectTo<ImpedimentoTarefaDTO>(_mapper.ConfigurationProvider)
            .FirstOrDefaultAsync(cancellationToken);

        if (impedimentoTarefa is null)
        {
            return new GetImpedimentoTarefaViewModel { OperationResult = OperationResult.NotFound };
        }

        return new GetImpedimentoTarefaViewModel { ImpedimentoTarefa = impedimentoTarefa, OperationResult = OperationResult.Success };
    }
}